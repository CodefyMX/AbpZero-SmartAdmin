namespace Cinotam.AbpModuleZero.Web.Areas.SysAdmin.Models
{
    public class GetConfigurationsByNameInput
    {
        public string[] SettingNames { get; set; }
        public string ConfigurationName { get; set; }

    }
}