using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Languages.Dto
{
    public class LanguageTextsForEditView
    {
        public List<string> Source { get; set; }

        public List<LanguageSelected> SourceLanguages { get; set; }
        public List<LanguageSelected> TargetLanguages { get; set; }
        public string SelectedSourceLanguage { get; set; }
        public string SelectedTargetLanguage { get; set; }

    }
}
