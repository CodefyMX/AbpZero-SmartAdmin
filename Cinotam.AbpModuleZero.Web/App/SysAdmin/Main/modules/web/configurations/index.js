(function() {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.configurations.index', ConfigurationsController);

    ConfigurationsController.$inject = ['abp.services.app.settings'];
    function ConfigurationsController(_settingsService) {
        var vm = this;
        //Define the configs 
        var isBoolean = function(string) {
            console.log(string);
            if (string == 'true' || string === true) {

                return true;
            }
            else {
                if (string == 'false' || string === false) {
                    return true;
                }
                return false;
            }
        }
        vm.isBoolean = function(settingValue) {
            var result = isBoolean(settingValue);
            if (result) {
                console.log(settingValue, 'is boolean');
                return true;
            }
            else {
                console.log(settingValue, 'is not boolean');
                return false;
            }
        }

        vm.configurationRequests = [
            {
                settingNames: [
                    "Abp.Net.Mail.Smtp.Host",
                    "Abp.Net.Mail.Smtp.Port",
                    "Abp.Net.Mail.Smtp.UserName",
                    "Abp.Net.Mail.Smtp.Password",
                    "Abp.Net.Mail.Smtp.Domain",
                    "Abp.Net.Mail.Smtp.EnableSsl",
                    "Abp.Net.Mail.Smtp.UseDefaultCredentials",
                    "Abp.Net.Mail.DefaultFromAddress",
                    "Abp.Net.Mail.DefaultFromDisplayName"
                ],
                configurationName: App.localize('EmailSettings')
            },
            {
                settingNames: [
                    "Abp.Localization.DefaultLanguageName",
                    "Abp.Timing.TimeZone",
                    "Abp.Zero.OrganizationUnits.MaxUserMembershipCount",
                    "Abp.WebApi.Runtime.Caching.ClearPassword",
                    "WebSiteStatus",
                    "UseSmtp",
                    "AppMode"
                ],
                configurationName: App.localize('Other')
            },
            {
                settingNames: [
                    "Abp.Zero.UserManagement.IsEmailConfirmationRequiredForLogin",
                    "Abp.Zero.UserManagement.TwoFactorLogin.IsEnabled",
                    "Abp.Zero.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled",
                    "Abp.Zero.UserManagement.TwoFactorLogin.IsEmailProviderEnabled",
                    "Abp.Zero.UserManagement.TwoFactorLogin.IsSmsProviderEnabled",
                    "Abp.Zero.UserManagement.UserLockOut.IsEnabled",
                    "Abp.Zero.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout",
                    "Abp.Zero.UserManagement.UserLockOut.DefaultAccountLockoutSeconds",
                    ""
                ],
                configurationName: App.localize('Account')
            },

        ]
        var configurations = [];
        vm.configForms = [];
        activate();

        ////////////////

        function activate() {
            // lol !! async madness
            vm.configurationRequests.forEach(function(element) {
                configurations = [];
                element.settingNames.forEach(function(setting) {
                    configurations.push(setting);
                });
                _settingsService.getSettingsOptions(configurations).then(function(response) {
                    //Build tabs
                    var newFormElement = new formElement(element.configurationName);
                    response.data.settings.forEach(function(settingElement) {
                        newFormElement.addSetting(settingElement);
                    });
                    vm.configForms.push(newFormElement);
                });
            });
        }
        vm.submit = function() {
            var data = [];
            vm.configForms.forEach(function(config) {
                config.settings.forEach(function(elm) {
                    data.push({
                        key: elm.key,
                        value: elm.value,
                        settingScopes: elm.settingScopes
                    });
                });
                console.log(data);
            });
            _settingsService.createEditSetting(data).then(function() {
                abp.notify.success(App.localize("ChangesSaved"), App.localize("Success"));
            });
        }
        vm.clearCaches = function() {
            _settingsService.clearCaches().then(function() {
                abp.notify.success(App.localize("CacheCleaned"), App.localize("Success"));
            });
        }
        var formElement = function(name, settings) {
            this.name = name;
            this.settings = [];

            this.addSetting = function(setting) {
                var isBoolean = vm.isBoolean(setting.value);
                if (isBoolean) {
                    setting.value = (setting.value === 'true');
                    this.settings.push(setting);
                }
                else {
                    this.settings.push(setting);
                }
            }
            return this;
        }

    }
})();