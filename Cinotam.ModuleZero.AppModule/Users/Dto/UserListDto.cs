using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Cinotam.AbpModuleZero.Users;
using System;

namespace Cinotam.ModuleZero.AppModule.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserListDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public DateTime? LastLoginTime { get; set; }


        public string LastLoginTimeString => LastLoginTime?.ToShortDateString() ?? "";

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }
        public string CreationTimeString => CreationTime.ToShortDateString();

        public bool IsLockoutEnabled { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsTwoFactorEnabled { get; set; }
        public virtual DateTime? LockoutEndDateUtc { get; set; }
    }
}