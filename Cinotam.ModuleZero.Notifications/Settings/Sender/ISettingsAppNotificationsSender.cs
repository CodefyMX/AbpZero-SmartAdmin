using Abp.Domain.Services;
using Cinotam.AbpModuleZero.Users;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.Notifications.Settings.Sender
{
    public interface ISettingsAppNotificationsSender : IDomainService
    {
        Task SendSettingsChangedNotification(int? tenantId, User changedBy);
    }
}
