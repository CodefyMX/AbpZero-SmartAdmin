using Abp;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Threading;
using Abp.UI.Inputs;
using Cinotam.AbpModuleZero.Editions;
using Cinotam.AbpModuleZero.MultiTenancy;
using Cinotam.ModuleZero.AppModule.Features.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Features
{
    public class FeatureService : CinotamModuleZeroAppServiceBase, IFeatureService
    {
        private readonly EditionManager _editionManager;
        private readonly TenantManager _tenantManager;
        private readonly IRepository<Edition> _editionRepository;
        public FeatureService(EditionManager editionManager, TenantManager tenantManager, IRepository<Edition> editionRepository)
        {
            _editionManager = editionManager;
            _tenantManager = tenantManager;
            _editionRepository = editionRepository;
        }

        public async Task CreateEdition(NewEditionInput input)
        {
            if (input.Id == 0)
            {
                var newEdition = new CinotamEdition { DisplayName = input.DisplayName, Price = input.Price };
                await _editionManager.CreateAsync(newEdition);
                await CurrentUnitOfWork.SaveChangesAsync();
                await SetFeatureValues(newEdition, input.Features);
            }
            else
            {
                var edition = await _editionManager.GetByIdAsync(input.Id);

                var mapped = input.MapTo(edition);

                await _editionRepository.UpdateAsync(mapped);

                await SetFeatureValues(mapped, input.Features);
            }

        }

        private async Task SetFeatureValues(Edition edition, IEnumerable<FeatureDto> inputFeatures)
        {
            var features =
            inputFeatures.Where(a => !string.IsNullOrEmpty(a.Name)).Select(GetValueName).ToArray();
            await _editionManager.SetFeatureValuesAsync(edition.Id, features);
        }

        private NameValue GetValueName(FeatureDto featureDto)
        {
            if (featureDto.Selected)
            {
                return new NameValue(featureDto.Name, featureDto.DefaultValue);
            }
            if (!(featureDto.InputType is SingleLineStringInputType))
                return new NameValue(featureDto.Name, DefaultBooleanValue);
            var feature = _editionManager.FeatureManager.GetOrNull(featureDto.Name);
            return new NameValue(featureDto.Name, feature.DefaultValue);
        }

        private const string DefaultBooleanValue = "false";
        public async Task SetFeatureValue(FeatureDto input)
        {
            await _editionManager.SetFeatureValueAsync(input.EditionId, input.Name, input.DefaultValue);
        }

        public EditionsOutput GetEditions()
        {
            var editions = _editionManager.Editions.ToList();
            return new EditionsOutput()
            {
                Editions = editions.Select(a => a.MapTo<EditionDto>()).ToList()
            };
        }

        public async Task<NewEditionInput> GetEditionForEdit(int? id)
        {
            if (!id.HasValue)
            {
                return new NewEditionInput()
                {
                    Features = GetAllFeatures(),
                };
            }
            var edition = await _editionManager.GetByIdAsync(id.Value);

            var editionInput = edition.MapTo<NewEditionInput>();

            editionInput.Features = GetAllFeatures(edition.Id);

            return editionInput;
        }

        public async Task DeleteEdition(DeleteEditionInput input)
        {
            var edition = await _editionManager.FindByIdAsync(input.EditionId);
            await _editionManager.DeleteAsync(edition);
        }

        private List<FeatureDto> GetAllFeatures(int? id = null)
        {
            var featuresFromDb = _editionManager.FeatureManager.GetAll().Where(a => a.Parent == null).ToList();
            var featuresResult = featuresFromDb.Select(a => new FeatureDto()
            {
                DefaultValue = GetDefaultValue(id, a.Name),
                EditionId = 0,
                Name = a.Name,
                Selected = IsEnabledInEdition(id, a.Name),
                InputType = a.InputType,
                ChildFeatures = GetChildrens(a.Children, id)
            }).ToList();
            return featuresResult;

        }

        private string GetDefaultValue(int? id, string argName)
        {
            if (!id.HasValue)
            {
                return _editionManager.FeatureManager.GetOrNull(argName).DefaultValue;
            }
            var value = AsyncHelper.RunSync(() => _editionManager.GetFeatureValueOrNullAsync(id.Value, argName));

            return value ?? _editionManager.FeatureManager.GetOrNull(argName).DefaultValue;
        }

        private bool IsEnabledInEdition(int? id, string featureName)
        {
            if (!id.HasValue) return false;
            var feature = AsyncHelper.RunSync(() => _editionManager.GetFeatureValueOrNullAsync(id.Value, featureName));
            return feature != null;
        }
        private List<FeatureDto> GetChildrens(IReadOnlyList<Feature> argChildren, int? id)
        {
            var listFeatureDto = new List<FeatureDto>();
            foreach (var argChild in argChildren)
            {
                listFeatureDto.Add(new FeatureDto()
                {
                    Name = argChild.Name,
                    InputType = argChild.InputType,
                    Selected = IsEnabledInEdition(id, argChild.Name),
                    DefaultValue = GetDefaultValue(id, argChild.Name),
                    ChildFeatures = GetChildrens(argChild.Children, id)
                });
            }
            return listFeatureDto;
        }
        public async Task SetEditionForTenant(int tenantId, int editionId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);
            var editionFeatureValues = await _editionManager.GetFeatureValuesAsync(editionId);
            await _tenantManager.SetFeatureValuesAsync(tenant.Id, editionFeatureValues.ToArray());
        }
    }
}
