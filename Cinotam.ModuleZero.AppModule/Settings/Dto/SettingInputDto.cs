using Abp.Application.Services.Dto;

namespace Cinotam.ModuleZero.AppModule.Settings.Dto
{
    public class SettingInputDto : EntityDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
