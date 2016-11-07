using Abp.Application.Services;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.ModuleZero.AppModule.Notifications.Dto;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Notifications
{
    public interface INotificationService : IApplicationService
    {
        Task<NotificationOutput> GetMyNotifications(int rowsPerPage, int? page, string search);
        Task<ReturnModel<NotificationDto>> GetMyNotificationsTable(RequestModel<object> requestModel);
    }
}
