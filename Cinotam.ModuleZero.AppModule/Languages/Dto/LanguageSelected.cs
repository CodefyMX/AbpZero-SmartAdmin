namespace Cinotam.ModuleZero.AppModule.Languages.Dto
{
    public class LanguageSelected
    {
        public LanguageSelected(string displayName, string name, string icon)
        {
            DisplayName = displayName;
            Name = name;
            Icon = icon;
        }

        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public string Icon { get; private set; }
    }
}