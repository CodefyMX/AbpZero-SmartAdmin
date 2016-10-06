using Abp;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.AutoMapper;
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
        public FeatureService(EditionManager editionManager, TenantManager tenantManager)
        {
            _editionManager = editionManager;
            _tenantManager = tenantManager;
        }

        public async Task CreateEdition(NewEditionInput input)
        {
            var newEdition = new CinotamEdition { DisplayName = input.DisplayName, Price = input.Price };
            await _editionManager.CreateAsync(newEdition);
            await CurrentUnitOfWork.SaveChangesAsync();
            await SetFeatureValues(newEdition, input.Features);

        }

        private async Task SetFeatureValues(Edition edition, IEnumerable<FeatureDto> inputFeatures)
        {
            var features =
            inputFeatures.Where(a => a.Selected).Select(a => new NameValue(a.Name, a.DefaultValue)).ToArray();
            await _editionManager.SetFeatureValuesAsync(edition.Id, features);
        }

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

        private List<FeatureDto> GetAllFeatures(int? id = null)
        {
            var featuresFromDb = _editionManager.FeatureManager.GetAll().Where(a => a.Parent == null).ToList();
            var featuresResult = featuresFromDb.Select(a => new FeatureDto()
            {
                DefaultValue = a.DefaultValue,
                EditionId = 0,
                Name = a.Name,
                Selected = IsEnabledInEdition(id, a.Name),
                InputType = a.InputType,
                ChildFeatures = GetChildrens(a.Children)
            }).ToList();
            return featuresResult;

        }

        private bool IsEnabledInEdition(int? id, string featureName)
        {
            if (!id.HasValue) return false;
            var feature = AsyncHelper.RunSync(() => _editionManager.GetFeatureValueOrNullAsync(id.Value, featureName));
            return feature != null;
        }

        private List<FeatureDto> GetChildrens(string argChildren)
        {
            var listFeatureDto = new List<FeatureDto>();
            var feature = _editionManager.FeatureManager.GetOrNull(argChildren);
            foreach (var featureChild in feature.Children)
            {
                listFeatureDto.Add(new FeatureDto()
                {
                    Name = featureChild.Name,
                    InputType = featureChild.InputType,
                    Selected = true,
                    DefaultValue = featureChild.DefaultValue,
                    ChildFeatures = GetChildrens(featureChild.Children)
                });
            }
            return listFeatureDto;
        }

        private List<FeatureDto> GetChildrens(IReadOnlyList<Feature> argChildren)
        {
            var listFeatureDto = new List<FeatureDto>();
            foreach (var argChild in argChildren)
            {
                listFeatureDto.Add(new FeatureDto()
                {
                    Name = argChild.Name,
                    InputType = argChild.InputType,
                    Selected = true,
                    DefaultValue = argChild.DefaultValue,
                    ChildFeatures = GetChildrens(argChild.Children)
                });
            }
            return listFeatureDto;
        }

        private IInputType GetInputType(string argName)
        {
            var feature = _editionManager.FeatureManager.GetOrNull(argName);
            return feature.InputType;
        }

        public async Task SetEditionForTenant(int tenantId, int editionId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);
            var editionFeatureValues = await _editionManager.GetFeatureValuesAsync(editionId);
            await _tenantManager.SetFeatureValuesAsync(tenant.Id, editionFeatureValues.ToArray());
        }
    }
}
