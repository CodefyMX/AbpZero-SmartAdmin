using Castle.Components.DictionaryAdapter;
using Cinotam.ModuleZero.AppModule.Features.Dto;

namespace Cinotam.ModuleZero.AppModule.MultiTenancy.Dto
{
    public class SetTenantEditionInput
    {
        public SetTenantEditionInput()
        {
            Editions = new EditionsOutput { Editions = new EditableList<EditionDto>() };
        }
        public int TenantId { get; set; }
        public int EditionId { get; set; }
        public EditionsOutput Editions { get; set; }
    }
}
