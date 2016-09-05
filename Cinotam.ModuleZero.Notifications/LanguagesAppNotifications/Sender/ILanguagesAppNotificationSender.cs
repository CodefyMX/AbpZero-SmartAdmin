using System.Threading.Tasks;
using Abp.Domain.Services;
using Abp.Localization;
using Cinotam.AbpModuleZero.Users;

namespace Cinotam.ModuleZero.Notifications.LanguagesAppNotifications.Sender
{
    public interface ILanguagesAppNotificationSender : IDomainService
    {
        Task SendLanguageCreatedNotification(ApplicationLanguage languageCreated, User user);
        Task SendLanguageDeletedNotification(ApplicationLanguage languageDeleted, User user);
    }
}
