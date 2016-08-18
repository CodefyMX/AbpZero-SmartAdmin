using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Settings.Dto
{
    public class SettingsOutput
    {
        public IEnumerable<SettingInputDto> Settings { get; set; }
    }
}
