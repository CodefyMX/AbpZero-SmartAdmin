using Abp.Configuration;
using Cinotam.ModuleZero.AppModule.Settings;
using Cinotam.ModuleZero.AppModule.Settings.Dto;
using Cinotam.ModuleZero.Notifications.Notifications;
using Shouldly;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cinotam.AbpModuleZero.Tests.Settings
{
    public class SettingsAppService_Test : AbpModuleZeroTestBase
    {
        private readonly ISettingsAppService _settingsAppService;

        public SettingsAppService_Test()
        {
            _settingsAppService = Resolve<ISettingsAppService>();
        }
        [Fact]
        public async Task CreateEditSetting_Test()
        {
            LoginAsHostAdmin();

            var settingInputDto = new SettingInputDto()
            {
                Key = "Theme",
                Value = "smart-style-0",
                SettingScopes = SettingScopes.User
            };

            await _settingsAppService.CreateEditSetting(new List<SettingInputDto>() { settingInputDto });

        }

        [Fact]
        public async Task GetSettingForEdit_Test()
        {
            LoginAsHostAdmin();
            var setting = await _settingsAppService.GetSettingForEdit("Theme");
            setting.ShouldNotBeNull();

        }

        [Fact]
        public async Task GetSettingsOptions_Test()
        {
            LoginAsHostAdmin();
            var settings = await _settingsAppService.GetSettingsOptions();
            settings.ShouldNotBeNull();
            settings.ShouldBeAssignableTo<SettingsOutput>();
            settings.Settings.ShouldNotBeNull();
            settings.Settings.ShouldBeAssignableTo<IEnumerable>();
        }

        [Fact]
        public async Task ChangeTheme_Test()
        {
            LoginAsHostAdmin();
            await _settingsAppService.ChangeTheme("smart-style-0");
        }

        [Fact]
        public async Task SubscribeToNotification_Test()
        {
            LoginAsHostAdmin();
            await _settingsAppService.SubscribeToNotification(NotificationNames.UserDeleted);
        }

        [Fact]
        public async Task IsSubscribed_Test()
        {
            LoginAsHostAdmin();
            var result = await _settingsAppService.IsSubscribed(NotificationNames.UserDeleted);
            result.ShouldNotBeNull();
            result.ShouldBeAssignableTo<bool>();
        }

        [Fact]
        public async Task UnSubscribeToNotification()
        {
            LoginAsHostAdmin();
            await _settingsAppService.UnSubscribeToNotification(NotificationNames.UserDeleted);
        }
    }
}
