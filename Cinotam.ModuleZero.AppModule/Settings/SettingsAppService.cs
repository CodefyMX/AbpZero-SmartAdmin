using Abp;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Localization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.ModuleZero.AppModule.Settings.Dto;
using Cinotam.ModuleZero.Notifications.GeneralSubscriber;
using Cinotam.ModuleZero.Notifications.UsersAppNotifications.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Settings
{
    [AbpAuthorize(PermissionNames.PagesSysAdminConfiguration)]
    public class SettingsAppService : CinotamModuleZeroAppServiceBase, ISettingsAppService
    {
        private readonly SettingManager _settingManager;
        private readonly ISettingDefinitionManager _definitionManager;
        private readonly ILocalizationContext _localizationContext;
        private readonly IAppNotificationsSubscriber _userAppNotificationsSubscriber;
        public SettingsAppService(SettingManager settingManager, ISettingDefinitionManager definitionManager, ILocalizationContext localizationContext, IAppNotificationsSubscriber userAppNotificationsSubscriber)
        {
            _settingManager = settingManager;
            _definitionManager = definitionManager;
            _localizationContext = localizationContext;
            _userAppNotificationsSubscriber = userAppNotificationsSubscriber;
        }

        public async Task CreateEditSetting(List<SettingInputDto> input)
        {
            foreach (var settingInputDto in input)
            {
                switch (settingInputDto.SettingScopes)
                {
                    case SettingScopes.Application:
                        await _settingManager.ChangeSettingForApplicationAsync(settingInputDto.Key, settingInputDto.Value);
                        break;
                    case SettingScopes.Application | SettingScopes.User | SettingScopes.Tenant:
                        await _settingManager.ChangeSettingForApplicationAsync(settingInputDto.Key, settingInputDto.Value);
                        break;
                    case SettingScopes.Application | SettingScopes.Tenant:
                        await _settingManager.ChangeSettingForApplicationAsync(settingInputDto.Key, settingInputDto.Value);
                        break;
                    case SettingScopes.Tenant:

                        break;
                    case SettingScopes.User:
                        if (AbpSession.UserId != null)
                            await _settingManager.ChangeSettingForUserAsync(AbpSession.UserId.Value, settingInputDto.Key, settingInputDto.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public async Task<SettingInputDto> GetSettingForEdit(string name)
        {
            var setting = await _settingManager.GetSettingValueAsync(name);
            return new SettingInputDto()
            {
                Key = name,
                Value = setting,
            };
        }

        public async Task<SettingsOutput> GetSettingsOptions()
        {
            var settings = _definitionManager.GetAllSettingDefinitions().Where(a => a.Scopes != SettingScopes.User);
            var settingsList = new List<SettingInputDto>();
            var output = new SettingsOutput();
            foreach (var settingDefinition in settings)
            {
                var value = await _settingManager.GetSettingValueAsync(settingDefinition.Name);
                var setting = new SettingInputDto()
                {
                    Key = settingDefinition.Name,
                    DisplayName = settingDefinition.DisplayName != null ? settingDefinition.DisplayName.Localize(_localizationContext) : settingDefinition.Name,
                    Value = value,
                    DefaultValue = settingDefinition.DefaultValue,
                    Description = settingDefinition.Description != null ? settingDefinition.Description.Localize(_localizationContext) : "",
                    SettingScopes = settingDefinition.Scopes
                };
                settingsList.Add(setting);
            }
            output.Settings = settingsList;
            return output;
        }
        /// <summary>
        /// Gets the settings definitions by names
        /// </summary>
        /// <param name="settingNames"></param>
        /// <returns></returns>
        public async Task<SettingsOutput> GetSettingsOptions(params string[] settingNames)
        {
            var settings = _definitionManager.GetAllSettingDefinitions().Where(a => a.Scopes != SettingScopes.User).ToList();
            var settingsList = new List<SettingInputDto>();
            var output = new SettingsOutput();
            foreach (var settingName in settingNames)
            {
                var settingDefinition = settings.FirstOrDefault(a => a.Name == settingName);
                if (settingDefinition == null) continue;
                var value = await _settingManager.GetSettingValueAsync(settingDefinition.Name);
                var setting = new SettingInputDto()
                {
                    Key = settingDefinition.Name,
                    DisplayName = settingDefinition.DisplayName != null ? settingDefinition.DisplayName.Localize(_localizationContext) : settingDefinition.Name,
                    Value = value,
                    DefaultValue = settingDefinition.DefaultValue,
                    Description = settingDefinition.Description != null ? settingDefinition.Description.Localize(_localizationContext) : "",
                    SettingScopes = settingDefinition.Scopes
                };
                settingsList.Add(setting);
            }
            output.Settings = settingsList;
            return output;
        }
        public async Task ChangeTheme(string themeName)
        {
            if (AbpSession.UserId != null)
            {
                await _settingManager.ChangeSettingForUserAsync(AbpSession.UserId.Value,
                    CinotamModuleZeroConsts.Theme, themeName);
            }
        }

        public async Task SubscribeToNotification(string notificationName)
        {
            if (AbpSession.UserId != null)
                await _userAppNotificationsSubscriber.SubscribeToNotification(new NotificationSubscriptionInput()
                {
                    UserIdentifier = new UserIdentifier(AbpSession.TenantId, AbpSession.UserId.Value),
                    NotificationName = notificationName
                });
        }
        public async Task UnSubscribeToNotification(string notificationName)
        {
            if (AbpSession.UserId != null)
                await _userAppNotificationsSubscriber.UnSubscribeToNotification(new NotificationSubscriptionInput()
                {
                    UserIdentifier = new UserIdentifier(AbpSession.TenantId, AbpSession.UserId.Value),
                    NotificationName = notificationName
                });
        }

        public async Task EditSetting(SettingInputDto input)
        {
            switch (input.SettingScopes)
            {
                case SettingScopes.Application:
                    await _settingManager.ChangeSettingForApplicationAsync(input.Key, input.Value);
                    break;
                case SettingScopes.Application | SettingScopes.User | SettingScopes.Tenant:
                    await _settingManager.ChangeSettingForApplicationAsync(input.Key, input.Value);
                    break;
                case SettingScopes.Application | SettingScopes.Tenant:
                    await _settingManager.ChangeSettingForApplicationAsync(input.Key, input.Value);
                    break;
                case SettingScopes.Tenant:

                    break;
                case SettingScopes.User:
                    if (AbpSession.UserId != null)
                        await _settingManager.ChangeSettingForUserAsync(AbpSession.UserId.Value, input.Key, input.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task<bool> IsSubscribed(string notificationName)
        {
            var result = AbpSession.UserId != null &&
                         await
                             _userAppNotificationsSubscriber.IsSubscribed(
                                 new UserIdentifier(AbpSession.TenantId, AbpSession.UserId.Value), notificationName);
            return result;
        }
    }
}
