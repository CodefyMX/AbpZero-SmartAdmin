using Abp.Application.Services.Dto;

namespace Cinotam.ModuleZero.AppModule.Settings.Dto
{
    public class SettingInputDto : EntityDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string DefaultValue { get; set; }
    }
}
