using Abp.Application.Services;
using Cinotam.ModuleZero.AppModule.Settings.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Settings
{
    public interface ISettingsAppService : IApplicationService
    {
        Task CreateEditSetting(List<SettingInputDto> input);
        Task<SettingInputDto> GetSettingForEdit(string name);
        Task<SettingsOutput> GetSettingsOptions();
        Task ChangeTheme(string themeName);
        Task SubscribeToNotification(string notificationName);
        Task<bool> IsSubscribed(string notificationName);
        Task UnSubscribeToNotification(string notificationName);
        Task EditSetting(SettingInputDto input);

        /// <summary>
        /// Gets the settings definitions by names
        /// </summary>
        /// <param name="settingNames"></param>
        /// <returns></returns>
        Task<SettingsOutput> GetSettingsOptions(params string[] settingNames);
    }
}
