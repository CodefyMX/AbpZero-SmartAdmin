using System;

namespace Cinotam.ModuleZero.AppModule.Languages.Dto
{
    public class LanguageDto
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Value { get; set; }
        public string Icon { get; set; }
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string CreationTimeString => CreationTime.ToShortDateString();
        public string DisplayName { get; set; }
        public bool IsStatic { get; set; }
    }
}