using Cinotam.ModuleZero.AppModule.Settings.Dto;

namespace Cinotam.ModuleZero.AppModule.Settings
{
    public interface ISettingsAppService
    {
        void CreateEditSetting(SettingInputDto input);
        SettingInputDto GetSettingForEdit(int id);

        SettingsOutput GetSettings();

    }
}
