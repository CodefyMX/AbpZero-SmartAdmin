using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Localization;
using Abp.Threading;
using Abp.UI;
using Abp.Web.Models;
using Abp.Web.Security.AntiForgery;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Authorization.Roles;
using Cinotam.AbpModuleZero.MultiTenancy;
using Cinotam.AbpModuleZero.Users;
using Cinotam.AbpModuleZero.Web.Controllers.Results;
using Cinotam.AbpModuleZero.Web.Models.Account;
using Cinotam.ModuleZero.AppModule.Users;
using Cinotam.TwoFactorAuth.Contracts;
using Cinotam.TwoFactorSender.Sender;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Controllers
{
    public class AccountController : AbpModuleZeroControllerBase
    {
        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly LogInManager _logInManager;
        private readonly ITwoFactorMessageService _twoFactorMessageService;
        private readonly IUserAppService _userAppService;
        private const string IsEmailConfString = "Abp.Zero.UserManagement.TwoFactorLogin.IsEmailProviderEnabled";

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public AccountController(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            IUnitOfWorkManager unitOfWorkManager,
            IMultiTenancyConfig multiTenancyConfig,
            LogInManager logInManager, ITwoFactorMessageService twoFactorMessageService, IUserAppService userAppService)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWorkManager = unitOfWorkManager;
            _multiTenancyConfig = multiTenancyConfig;
            _logInManager = logInManager;
            _twoFactorMessageService = twoFactorMessageService;
            _userAppService = userAppService;
            _userManager.SmsService = twoFactorMessageService;
            _userManager.UserTokenProvider = new EmailTokenProvider<User, long>();
        }

        #region Login / Logout

        public ActionResult Login(string message, string returnUrl = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }
            ViewBag.Message = message;
            return View(
                new LoginFormViewModel
                {
                    ReturnUrl = returnUrl,
                    IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                    IsAValidTenancyNameInUrl = UrlHasAValidTenancyName,
                    TenancyName = UrlHasAValidTenancyName ? GetTenancyNameFromSession : string.Empty
                });
        }
        public ActionResult PhoneNumberVerification(string returnUrl, long userId, string provider)
        {
            if (PhoneNumber == null) throw new UserFriendlyException(L("SessionExpired"));
            return View(new UserTwoFactorVerificationInput()
            {
                Provider = provider,
                ReturnUrl = returnUrl,
                UserId = userId,
                PhoneNumber = PhoneNumber.ToString()
            });
        }
        public async Task<ActionResult> EmailConfirmation(long userId, string token)
        {
            var user = await _userManager.GetUserByIdAsync(userId);

            await _userManager.ConfirmEmailAsync(user.Id, token);


            return RedirectToAction("Login", new { message = "EmailConfirmed" });

        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(a => a.EmailAddress == email);
            if (user == null) return View("RecoveryMessageSent");
            var resetCode = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
            var resetUrl = Url.Action("ResetPasswordForm", new { token = resetCode, userId = user.Id });
            var strUrl = ServerUrl;
            var trueConfirmationUrl = "  <a href='" + strUrl + resetUrl + "'>" + L("ClickHereToResetPassword") + "<a>";
            await _userAppService.SendPasswordResetCode(trueConfirmationUrl, user.EmailAddress);

            return View("RecoveryMessageSent");
        }
        public ActionResult EmailVerification(string returnUrl, long userId, string provider)
        {
            if (Email == null) throw new UserFriendlyException(L("SessionExpired"));
            return View(new UserTwoFactorVerificationInput()
            {
                Provider = provider,
                ReturnUrl = returnUrl,
                UserId = userId,
                Email = Email.ToString()
            });
        }
        private const string PhoneNumberKey = "PhoneNumber";
        private const string EmailKey = "EmailKey";
        private object PhoneNumber => Session[PhoneNumberKey];
        private object Email => Session[EmailKey];
        private void SetPhoneNumber(string phoneNumber)
        {
            Session[PhoneNumberKey] = phoneNumber;
        }
        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public async Task<JsonResult> PhoneNumberVerification(UserTwoFactorVerificationInput input)
        {

            var user = await _userManager.FindByIdAsync(input.UserId);
            if (user.TenantId != null) _userManager.RegisterTwoFactorProviders(user.TenantId.Value);
            else _userManager.RegisterTwoFactorProviders(null);
            await _userManager.GetValidTwoFactorProvidersAsync(input.UserId);
            var result = await _userManager.VerifyTwoFactorTokenAsync(input.UserId, input.Provider, input.Token);
            if (!result) return Json(new { Error = L("InvalidCode") });
            await SignInAsync(user);
            if (string.IsNullOrEmpty(input.ReturnUrl))
            {
                return Json(new AjaxResponse { TargetUrl = "/Home/Index" });
            }
            return Json(new AjaxResponse { TargetUrl = input.ReturnUrl });


        }
        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public async Task<ActionResult> EmailVerification(UserTwoFactorVerificationInput input)
        {
            var user = await _userManager.FindByIdAsync(input.UserId);
            if (user.TenantId != null) _userManager.RegisterTwoFactorProviders(user.TenantId.Value);
            else _userManager.RegisterTwoFactorProviders(null);
            await _userManager.GetValidTwoFactorProvidersAsync(input.UserId);
            var result = await _userManager.VerifyTwoFactorTokenAsync(input.UserId, input.Provider, input.Token);
            if (!result) return Json(new { Error = L("InvalidCode") });
            await SignInAsync(user);
            if (string.IsNullOrEmpty(input.ReturnUrl))
            {
                return Json(new AjaxResponse { TargetUrl = "/Home/Index" });
            }
            return Json(new AjaxResponse { TargetUrl = input.ReturnUrl });


        }

        [HttpPost]
        [DisableAuditing]
        public async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "", string returnUrlHash = "")
        {

            CheckModelState();

            var loginResult = await GetLoginResultAsync(
                loginModel.UsernameOrEmailAddress,
                loginModel.Password,
                loginModel.TenancyName
            );
            if (loginResult.Result == AbpLoginResultType.UserEmailIsNotConfirmed)
            {
                var url = Url.Action("EmailNotConfirmed", new { userName = loginModel.UsernameOrEmailAddress });

                return Json(new AjaxResponse { TargetUrl = url });
            }

            CheckAndConfirmTwoFactorProviders(loginResult.User);

            if (await IsNecessaryToSendSms(loginResult.User))
            {
                const string smsProvider = "Sms";
                var code = await _userManager.GenerateTwoFactorTokenAsync(loginResult.User.Id, smsProvider);

                var messageResult = await _twoFactorMessageService.SendSmsMessage(new IdentityMessage()
                {
                    Body = code,
                    Destination = loginResult.User.CountryPhoneCode + loginResult.User.PhoneNumber,
                    Subject = LocalizationManager.GetString(LocalizationSourceName, "YourTwoFactorCode")
                });

                if (messageResult.SendStatus == SendStatus.Fail)
                    throw new UserFriendlyException(L("SendMessageFailed"));

                SetPhoneNumber(loginResult.User.PhoneNumber);
                var url = Url.Action("PhoneNumberVerification",
                    new
                    {
                        returnUrl,
                        userId = loginResult.User.Id,
                        provider = smsProvider,
                    });

                return Json(new AjaxResponse { TargetUrl = url });
            }
            if (IsTwoFactorEnabled(loginResult.User) && IsEmailRequiredForLogin && IsEmailProviderEnabled)
            {

                SetEmail(loginResult.User.EmailAddress);

                const string emailProvider = "Email";
                //Send email verification code

                var emailCode = await _userManager.GenerateTwoFactorTokenAsync(loginResult.User.Id, emailProvider);

                await _twoFactorMessageService.SendEmailMessage(new IdentityMessage()
                {
                    Body = emailCode,
                    Destination = loginResult.User.EmailAddress,
                    Subject = LocalizationManager.GetString(LocalizationSourceName, "YourTwoFactorCode")
                });

                var url = Url.Action("EmailVerification",
                    new
                    {
                        returnUrl,
                        userId = loginResult.User.Id,
                        provider = emailProvider,
                    });
                return Json(new AjaxResponse { TargetUrl = url });
            }
            await SignInAsync(loginResult.User, loginResult.Identity, loginModel.RememberMe);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            if (!string.IsNullOrWhiteSpace(returnUrlHash))
            {
                returnUrl = returnUrl + returnUrlHash;
            }

            return Json(new AjaxResponse { TargetUrl = returnUrl });
        }

        private void SetEmail(string userEmailAddress)
        {
            Session[EmailKey] = userEmailAddress;
        }


        public bool IsEmailProviderEnabled
            =>
            bool.Parse(SettingManager.GetSettingValue(IsEmailConfString));

        public bool IsEmailRequiredForLogin
            => bool.Parse(SettingManager.GetSettingValue(IsEmailConfString));

        private void CheckAndConfirmTwoFactorProviders(User loginResultUser)
        {
            if (loginResultUser.TenantId != null) _userManager.RegisterTwoFactorProviders(loginResultUser.TenantId.Value);
            else _userManager.RegisterTwoFactorProviders(null);
            _userManager.SmsService = _twoFactorMessageService;
        }


        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                case AbpLoginResultType.UserEmailIsNotConfirmed:
                    return loginResult;
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private async Task SendConfirmationMail(string userName, AbpLoginResultType loginResultType, string tenancyName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(a => a.UserName == userName) ??
                       await _userManager.Users.FirstOrDefaultAsync(a => a.EmailAddress == userName);
            if (user == null) throw CreateExceptionForFailedLoginAttempt(loginResultType, userName, tenancyName);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var confirmationUrl = Url.Action("EmailConfirmation", new
            {
                userId = user.Id,
                token = token
            });
            if (HttpContext.Request.Url != null)
            {
                var strUrl = ServerUrl;
                var trueConfirmationUrl = "  <a href='" + strUrl + confirmationUrl + "'>" + L("ClickHereToConfirm") + "<a>";
                await _userAppService.SendEmailConfirmationCode(trueConfirmationUrl, user.EmailAddress);
            }

        }

        private async Task SignInAsync(User user, ClaimsIdentity identity = null, bool rememberMe = false)
        {
            if (identity == null)
            {
                identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
        }

        private Exception CreateExceptionForFailedLoginAttempt(AbpLoginResultType result, string usernameOrEmailAddress, string tenancyName)
        {
            switch (result)
            {
                case AbpLoginResultType.Success:
                    return new ApplicationException("Don't call this method with a success result!");
                case AbpLoginResultType.InvalidUserNameOrEmailAddress:
                case AbpLoginResultType.InvalidPassword:
                    return new UserFriendlyException(L("LoginFailed"), L("InvalidUserNameOrPassword"));
                case AbpLoginResultType.InvalidTenancyName:
                    return new UserFriendlyException(L("LoginFailed"), L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                case AbpLoginResultType.TenantIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("TenantIsNotActive", tenancyName));
                case AbpLoginResultType.UserIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("UserIsNotActiveAndCanNotLogin", usernameOrEmailAddress));
                case AbpLoginResultType.UserEmailIsNotConfirmed:
                    return new UserFriendlyException(L("LoginFailed"), "Your email address is not confirmed. You can not login"); //TODO: localize message
                default: //Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                    Logger.Warn("Unhandled login fail reason: " + result);
                    return new UserFriendlyException(L("LoginFailed"));
            }
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        #endregion

        #region Register

        public ActionResult Register()
        {
            return RegisterView(new RegisterViewModel());
        }

        private ActionResult RegisterView(RegisterViewModel model)
        {
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;

            return View("Register", model);
        }

        [HttpPost]
        [UnitOfWork]
        [DisableAbpAntiForgeryTokenValidation]
        public virtual async Task<ActionResult> Register(RegisterViewModel model)
        {
            try
            {
                CheckModelState();

                //Get tenancy name and tenant
                if (!_multiTenancyConfig.IsEnabled)
                {
                    model.TenancyName = Tenant.DefaultTenantName;
                }
                else if (model.TenancyName.IsNullOrEmpty())
                {
                    throw new UserFriendlyException(L("TenantNameCanNotBeEmpty"));
                }

                var tenant = await GetActiveTenantAsync(model.TenancyName);

                //Create user
                var user = new User
                {
                    TenantId = tenant.Id,
                    Name = model.Name,
                    Surname = model.Surname,
                    EmailAddress = model.EmailAddress,
                    IsActive = true
                };

                //Get external login info if possible
                ExternalLoginInfo externalLoginInfo = null;
                if (model.IsExternalLogin)
                {
                    externalLoginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
                    if (externalLoginInfo == null)
                    {
                        throw new ApplicationException("Can not external login!");
                    }

                    user.Logins = new List<UserLogin>
                    {
                        new UserLogin
                        {
                            TenantId = tenant.Id,
                            LoginProvider = externalLoginInfo.Login.LoginProvider,
                            ProviderKey = externalLoginInfo.Login.ProviderKey
                        }
                    };

                    if (model.UserName.IsNullOrEmpty())
                    {
                        model.UserName = model.EmailAddress;
                    }

                    model.Password = Users.User.CreateRandomPassword();

                    if (string.Equals(externalLoginInfo.Email, model.EmailAddress, StringComparison.InvariantCultureIgnoreCase))
                    {
                        user.IsEmailConfirmed = true;
                    }
                }
                else
                {
                    //Username and Password are required if not external login
                    if (model.UserName.IsNullOrEmpty() || model.Password.IsNullOrEmpty())
                    {
                        throw new UserFriendlyException(L("FormIsNotValid" +
                                                          "Message" +
                                                          ""));
                    }
                }

                user.UserName = model.UserName;
                user.Password = new PasswordHasher().HashPassword(model.Password);

                //Switch to the tenant
                _unitOfWorkManager.Current.EnableFilter(AbpDataFilters.MayHaveTenant); //TODO: Needed?
                _unitOfWorkManager.Current.SetTenantId(tenant.Id);

                //Add default roles
                user.Roles = new List<UserRole>();
                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    user.Roles.Add(new UserRole { RoleId = defaultRole.Id });
                }

                //Save user
                CheckErrors(await _userManager.CreateAsync(user));
                await _unitOfWorkManager.Current.SaveChangesAsync();

                //Directly login if possible
                if (user.IsActive)
                {
                    AbpLoginResult<Tenant, User> loginResult;
                    if (externalLoginInfo != null)
                    {
                        loginResult = await _logInManager.LoginAsync(externalLoginInfo.Login, tenant.TenancyName);
                    }
                    else
                    {
                        loginResult = await GetLoginResultAsync(user.UserName, model.Password, tenant.TenancyName);
                    }

                    if (loginResult.Result == AbpLoginResultType.Success)
                    {
                        await SignInAsync(loginResult.User, loginResult.Identity);
                        return Redirect(Url.Action("Index", "Home"));
                    }

                    Logger.Warn("New registered user could not be login. This should not be normally. login result: " + loginResult.Result);
                }

                //If can not login, show a register result page
                return View("RegisterResult", new RegisterResultViewModel
                {
                    TenancyName = tenant.TenancyName,
                    NameAndSurname = user.Name + " " + user.Surname,
                    UserName = user.UserName,
                    EmailAddress = user.EmailAddress,
                    IsActive = user.IsActive
                });
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
                ViewBag.ErrorMessage = ex.Message;

                return View("Register", model);
            }
        }

        #endregion

        #region External Login

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(
                provider,
                Url.Action(
                    "ExternalLoginCallback",
                    "Account",
                    new
                    {
                        ReturnUrl = returnUrl
                    })
                );
        }

        [UnitOfWork]
        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl, string tenancyName = "")
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            //Try to find tenancy name
            if (tenancyName.IsNullOrEmpty())
            {
                var tenants = await FindPossibleTenantsOfUserAsync(loginInfo.Login);
                switch (tenants.Count)
                {
                    case 0:
                        return await RegisterView(loginInfo);
                    case 1:
                        tenancyName = tenants[0].TenancyName;
                        break;
                    default:
                        return View("TenantSelection", new TenantSelectionViewModel
                        {
                            Action = Url.Action("ExternalLoginCallback", "Account", new { returnUrl }),
                            Tenants = tenants.MapTo<List<TenantSelectionViewModel.TenantInfo>>()
                        });
                }
            }

            var loginResult = await _logInManager.LoginAsync(loginInfo.Login, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    await SignInAsync(loginResult.User, loginResult.Identity, false);

                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        returnUrl = Url.Action("Index", "Home");
                    }

                    return Redirect(returnUrl);
                case AbpLoginResultType.UnknownExternalLogin:
                    return await RegisterView(loginInfo, tenancyName);
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, loginInfo.Email ?? loginInfo.DefaultUserName, tenancyName);
            }
        }

        private async Task<ActionResult> RegisterView(ExternalLoginInfo loginInfo, string tenancyName = null)
        {
            var name = loginInfo.DefaultUserName;
            var surname = loginInfo.DefaultUserName;

            var extractedNameAndSurname = TryExtractNameAndSurnameFromClaims(loginInfo.ExternalIdentity.Claims.ToList(), ref name, ref surname);

            var viewModel = new RegisterViewModel
            {
                TenancyName = tenancyName,
                EmailAddress = loginInfo.Email,
                Name = name,
                Surname = surname,
                IsExternalLogin = true
            };

            if (!tenancyName.IsNullOrEmpty() && extractedNameAndSurname)
            {
                return await Register(viewModel);
            }

            return RegisterView(viewModel);
        }

        [UnitOfWork]
        protected virtual async Task<List<Tenant>> FindPossibleTenantsOfUserAsync(UserLoginInfo login)
        {
            List<User> allUsers;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                allUsers = await _userManager.FindAllAsync(login);
            }

            return allUsers
                .Where(u => u.TenantId != null)
                .Select(u => AsyncHelper.RunSync(() => _tenantManager.FindByIdAsync(u.TenantId.Value)))
                .ToList();
        }

        private static bool TryExtractNameAndSurnameFromClaims(List<Claim> claims, ref string name, ref string surname)
        {
            string foundName = null;
            string foundSurname = null;

            var givennameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            if (givennameClaim != null && !givennameClaim.Value.IsNullOrEmpty())
            {
                foundName = givennameClaim.Value;
            }

            var surnameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
            if (surnameClaim != null && !surnameClaim.Value.IsNullOrEmpty())
            {
                foundSurname = surnameClaim.Value;
            }

            if (foundName == null || foundSurname == null)
            {
                var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (nameClaim != null)
                {
                    var nameSurName = nameClaim.Value;
                    if (!nameSurName.IsNullOrEmpty())
                    {
                        var lastSpaceIndex = nameSurName.LastIndexOf(' ');
                        if (lastSpaceIndex < 1 || lastSpaceIndex > (nameSurName.Length - 2))
                        {
                            foundName = foundSurname = nameSurName;
                        }
                        else
                        {
                            foundName = nameSurName.Substring(0, lastSpaceIndex);
                            foundSurname = nameSurName.Substring(lastSpaceIndex);
                        }
                    }
                }
            }

            if (!foundName.IsNullOrEmpty())
            {
                name = foundName;
            }

            if (!foundSurname.IsNullOrEmpty())
            {
                surname = foundSurname;
            }

            return foundName != null && foundSurname != null;
        }

        #endregion

        #region Common private methods
        private bool IsTwoFactorEnabled(User loginResultUser)
        {
            return loginResultUser.IsTwoFactorEnabled;
        }

        private async Task<bool> IsNecessaryToSendSms(User loginResultUser)
        {
            var isGlobalTwoFactorEnabled =
                bool.Parse(await SettingManager.GetSettingValueAsync("Abp.Zero.UserManagement.TwoFactorLogin.IsEnabled"));
            var isSmsTwoFactorEnabled =
                bool.Parse(
                    await
                        SettingManager.GetSettingValueAsync(
                            "Abp.Zero.UserManagement.TwoFactorLogin.IsSmsProviderEnabled"));
            var userHasTwoFactorEnabled = loginResultUser.IsTwoFactorEnabled;
            var userHasPhoneNumber = loginResultUser.PhoneNumber != "" && loginResultUser.IsPhoneNumberConfirmed;

            if (!isGlobalTwoFactorEnabled) return false;
            if (!isSmsTwoFactorEnabled) return false;
            return userHasPhoneNumber && userHasTwoFactorEnabled;
        }
        private async Task<Tenant> GetActiveTenantAsync(string tenancyName)
        {
            var tenant = await _tenantManager.FindByTenancyNameAsync(tenancyName);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIsNotActive", tenancyName));
            }

            return tenant;
        }

        #endregion

        public ActionResult EmailNotConfirmed(string userName)
        {
            return View(new EmailConfirmationInput()
            {
                UserName = userName
            });
        }

        public async Task<ActionResult> ResendMail(string userName)
        {
            await SendConfirmationMail(userName, AbpLoginResultType.UserEmailIsNotConfirmed, GetTenancyNameFromSession);

            return RedirectToAction("EmailNotConfirmed", new { userName = userName });

        }

        public ActionResult ResetPasswordForm(string token, long userid)
        {
            return View(new ResetPasswordInput()
            {
                Token = token,
                UserId = userid
            });
        }

        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]

        public async Task<JsonResult> ResetPasswordForm(ResetPasswordInput input)
        {
            var result = await _userManager.ResetPasswordAsync(input.UserId, input.Token, input.Password);
            if (result.Succeeded)
            {
                var url = Url.Action("Login", new { message = "PasswordChanged" });
                return Json(new AjaxResponse { TargetUrl = url });
            }
            throw new UserFriendlyException(L("SessionExpired"));
        }
    }
}