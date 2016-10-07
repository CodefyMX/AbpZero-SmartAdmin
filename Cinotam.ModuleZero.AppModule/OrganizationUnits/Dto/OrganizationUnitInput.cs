using Abp.Application.Services.Dto;

namespace Cinotam.ModuleZero.AppModule.OrganizationUnits.Dto
{
    public class OrganizationUnitInput : EntityDto<long>
    {
        public long? ParentId { get; set; }
        public string DisplayName { get; set; }
    }
}
