using Abp.Application.Editions;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Cinotam.AbpModuleZero.Editions;

namespace Cinotam.ModuleZero.AppModule.Features.Dto
{
    [AutoMap(typeof(Edition), typeof(CinotamEdition))]
    public class EditionDto : FullAuditedEntityDto
    {
        public string DisplayName { get; set; }
        public decimal Price { get; set; }
    }
}