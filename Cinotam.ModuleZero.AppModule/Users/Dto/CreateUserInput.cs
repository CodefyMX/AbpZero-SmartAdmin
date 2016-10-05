using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Cinotam.AbpModuleZero.Users;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cinotam.ModuleZero.AppModule.Users.Dto
{
    [AutoMap(typeof(User))]
    public class CreateUserInput : EntityDto<long>
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(User.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }
        public bool SendNotificationMail { get; set; }
        public bool IsLockoutEnabled { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsTwoFactorEnabled { get; set; }
        public virtual DateTime? LockoutEndDateUtc { get; set; }
    }
}