using Abp.AutoMapper;
using Cinotam.AbpModuleZero.Users;

namespace Cinotam.ModuleZero.Notifications.Chat.Outputs
{
    [AutoMap(typeof(User))]
    public class UserOutput
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }
        public int? TenantId { get; set; }
    }
}
