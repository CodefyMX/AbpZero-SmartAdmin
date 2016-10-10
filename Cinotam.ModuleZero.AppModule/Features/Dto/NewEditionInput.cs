using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.UI.Inputs;
using Castle.Components.DictionaryAdapter;
using Cinotam.AbpModuleZero.Editions;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Features.Dto
{
    [AutoMap(typeof(Edition), typeof(CinotamEdition))]
    public class NewEditionInput : FullAuditedEntityDto
    {
        public string DisplayName { get; set; }
        public List<FeatureDto> Features { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
    }
    [AutoMap(typeof(Feature))]
    public class FeatureDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Selected { get; set; }
        public string DefaultValue { get; set; }
        public int EditionId { get; set; }
        public IInputType InputType { get; set; }
        public List<FeatureDto> ChildFeatures { get; set; } = new EditableList<FeatureDto>();
    }
}
