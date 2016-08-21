using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Languages.Dto
{
    public class LanguageTextsForEdit
    {
        public string Source { get; set; }
        public List<LanguageText> SourceLanguageTexts { get; set; }
        public List<LanguageText> TargetLanguageTexts { get; set; }
    }
}
