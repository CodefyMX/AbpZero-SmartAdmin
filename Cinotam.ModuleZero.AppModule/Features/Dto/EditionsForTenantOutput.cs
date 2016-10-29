using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Features.Dto
{
    public class EditionsForTenantOutput
    {
        public List<EditionDtoCustom> Editions { get; set; } = new EditableList<EditionDtoCustom>();
        public int TenantId { get; set; }
    }

    public class EditionDtoCustom : EditionDto
    {
        public bool IsEnabledForTenant { get; set; }
    }
}
