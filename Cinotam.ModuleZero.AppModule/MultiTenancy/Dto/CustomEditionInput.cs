using Cinotam.ModuleZero.AppModule.Features.Dto;

namespace Cinotam.ModuleZero.AppModule.MultiTenancy.Dto
{
    public class CustomEditionInput : NewEditionInput
    {
        public int TenantId { get; set; }
    }
}
