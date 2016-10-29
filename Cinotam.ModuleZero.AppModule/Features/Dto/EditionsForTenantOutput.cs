using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;

namespace Cinotam.ModuleZero.AppModule.Features.Dto
{
    public class EditionsForTenantOutput
    {
        public List<EditionDtoCustom> Editions { get; set; } = new EditableList<EditionDtoCustom>();
    }

    public class EditionDtoCustom : EditionDto
    {
        public bool IsEnabledForTenant { get; set; }
    }
}
