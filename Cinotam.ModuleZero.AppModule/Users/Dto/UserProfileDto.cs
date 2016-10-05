using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Cinotam.AbpModuleZero.Users;
using System;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserProfileDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }
        public string ProfilePicture { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public List<string> MyRoles { get; set; }
        public string MyStringRoles => string.Join(",", MyRoles);
        public bool IsLockoutEnabled { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsTwoFactorEnabled { get; set; }
        public virtual DateTime? LockoutEndDateUtc { get; set; }
    }
}
