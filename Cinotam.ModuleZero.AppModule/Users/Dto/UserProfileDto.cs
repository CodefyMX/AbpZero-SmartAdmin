using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Cinotam.AbpModuleZero.Users;

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
    }
}
