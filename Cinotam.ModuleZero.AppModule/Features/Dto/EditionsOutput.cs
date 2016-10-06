using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Features.Dto
{
    public class EditionsOutput
    {
        public List<EditionDto> Editions { get; set; } = new EditableList<EditionDto>();
    }
}
