using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Languages.Dto
{
    public class LanguageTextsForEdit
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public List<LanguageText> SourceLanguageTexts { get; set; }
        public List<LanguageText> TargetLanguageTexts { get; set; }
    }

    public class LanguageText
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
