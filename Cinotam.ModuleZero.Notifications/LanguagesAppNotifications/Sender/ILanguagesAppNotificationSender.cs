using Abp.Localization;
using Cinotam.AbpModuleZero.Users;

namespace Cinotam.ModuleZero.Notifications.LanguagesAppNotifications.Sender
{
    public interface ILanguagesAppNotificationSender
    {
        void SendLanguageCreatedNotification(ApplicationLanguage languageCreated, User user);
        void SendLanguageDeletedNotification(ApplicationLanguage languageDeleted,User user);
    }
}
