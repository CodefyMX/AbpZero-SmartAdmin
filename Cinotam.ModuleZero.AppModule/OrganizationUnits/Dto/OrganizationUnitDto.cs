using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Organizations;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.OrganizationUnits.Dto
{
    [AutoMapFrom(typeof(OrganizationUnit))]
    public class OrganizationUnitDto : EntityDto<long>
    {
        public virtual long? ParentId { get; set; }
        public virtual string Code { get; set; }
        public virtual string DisplayName { get; set; }
        /// <summary>Children of this OU.</summary>
        public virtual List<OrganizationUnitDto> ChildrenDto { get; set; }
    }
}
