using Abp;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Notifications;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.ModuleZero.AppModule.Notifications.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Notifications
{
    [AbpAuthorize]
    public class NotificationService : CinotamModuleZeroAppServiceBase, INotificationService
    {
        private readonly UserNotificationManager _userNotificationManager;
        public NotificationService(UserNotificationManager userNotificationManager)
        {
            _userNotificationManager = userNotificationManager;
        }

        public async Task<NotificationOutput> GetMyNotifications(int rowsPerPage, int? page, string search)
        {
            if (AbpSession.UserId == null) return new NotificationOutput();
            var notifications = await _userNotificationManager.GetUserNotificationsAsync(new UserIdentifier(AbpSession.TenantId, AbpSession.UserId.Value));

            if (!string.IsNullOrEmpty(search))
            {
                notifications = notifications.Where(a => a.Notification.Data.ToString().Contains(search)).ToList();
            }

            return new NotificationOutput()
            {
                Notifications = notifications.Select(a => a.MapTo<NotificationDto>()).ToList()
            };
        }

        public async Task<ReturnModel<NotificationDto>> GetMyNotificationsTable(RequestModel<object> requestModel)
        {
            var notifications = await _userNotificationManager.GetUserNotificationsAsync(new UserIdentifier(AbpSession.TenantId, AbpSession.UserId.Value));
            return new ReturnModel<NotificationDto>()
            {
                data = notifications.Select(a => a.MapTo<NotificationDto>()).ToArray(),

            };
        }
    }
}
