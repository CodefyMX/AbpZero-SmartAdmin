using Abp.Authorization;
using Abp.Configuration;
using Abp.Localization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.ModuleZero.AppModule.Settings.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Settings
{
    [AbpAuthorize(PermissionNames.PagesSysAdminConfiguration)]
    public class SettingsAppService : CinotamModuleZeroAppServiceBase, ISettingsAppService
    {
        private readonly SettingManager _settingManager;
        private readonly ISettingDefinitionManager _definitionManager;
        private readonly ILocalizationContext _localizationContext;
        public SettingsAppService(SettingManager settingManager, ISettingDefinitionManager definitionManager, ILocalizationContext localizationContext)
        {
            _settingManager = settingManager;
            _definitionManager = definitionManager;
            _localizationContext = localizationContext;
        }

        public async Task CreateEditSetting(List<SettingInputDto> input)
        {
            foreach (var settingInputDto in input)
            {
                switch (settingInputDto.SettingScopes)
                {
                    case SettingScopes.Application:
                        await _settingManager.ChangeSettingForApplicationAsync(settingInputDto.Key, settingInputDto.Value);
                        break;
                    case SettingScopes.Application | SettingScopes.User | SettingScopes.Tenant:
                        await _settingManager.ChangeSettingForApplicationAsync(settingInputDto.Key, settingInputDto.Value);
                        break;
                    case SettingScopes.Application | SettingScopes.Tenant:
                        await _settingManager.ChangeSettingForApplicationAsync(settingInputDto.Key, settingInputDto.Value);
                        break;
                    case SettingScopes.Tenant:

                        break;
                    case SettingScopes.User:
                        if (AbpSession.UserId != null)
                            await _settingManager.ChangeSettingForUserAsync(AbpSession.UserId.Value, settingInputDto.Key, settingInputDto.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public async Task<SettingInputDto> GetSettingForEdit(string name)
        {
            var setting = await _settingManager.GetSettingValueAsync(name);
            return new SettingInputDto()
            {
                Key = name,
                Value = setting,
            };
        }

        public async Task<SettingsOutput> GetSettingsOptions()
        {
            var settings = _definitionManager.GetAllSettingDefinitions();
            var settingsList = new List<SettingInputDto>();
            var output = new SettingsOutput();
            foreach (var settingDefinition in settings)
            {
                var value = await _settingManager.GetSettingValueAsync(settingDefinition.Name);
                var setting = new SettingInputDto()
                {
                    Key = settingDefinition.Name,
                    DisplayName = settingDefinition.DisplayName != null ? settingDefinition.DisplayName.Localize(_localizationContext) : settingDefinition.Name,
                    Value = value,
                    DefaultValue = settingDefinition.DefaultValue,
                    Description = settingDefinition.Description != null ? settingDefinition.Description.Localize(_localizationContext) : "",
                    SettingScopes = settingDefinition.Scopes
                };
                settingsList.Add(setting);
            }
            output.Settings = settingsList;
            return output;
        }
    }
}
