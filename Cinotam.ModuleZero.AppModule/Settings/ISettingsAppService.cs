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
    }
}
