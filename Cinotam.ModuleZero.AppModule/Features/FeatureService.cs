using Abp;
using Abp.Application.Editions;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.UI.Inputs;
using Cinotam.AbpModuleZero.Editions;
using Cinotam.AbpModuleZero.MultiTenancy;
using Cinotam.AbpModuleZero.Users;
using Cinotam.ModuleZero.AppModule.Features.Dto;
using Cinotam.ModuleZero.AppModule.Features.FeatureManager;
using Cinotam.ModuleZero.Notifications.EditionNotifications.Sender;
using Cinotam.ModuleZero.Notifications.EditionNotifications.Subscriber;
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
        private readonly ICustomEditionManager _customEditionManager;
        private readonly IEditionNotificationsSender _editionNotificationsSender;
        private readonly IEditionNotificationSubscriber _editionNotificationSubscriber;
        public FeatureService(EditionManager editionManager, TenantManager tenantManager, IRepository<Edition> editionRepository, ICustomEditionManager customEditionManager, IEditionNotificationsSender editionNotificationsSender, IEditionNotificationSubscriber editionNotificationSubscriber)
        {
            _editionManager = editionManager;
            _tenantManager = tenantManager;
            _editionRepository = editionRepository;
            _customEditionManager = customEditionManager;
            _editionNotificationsSender = editionNotificationsSender;
            _editionNotificationSubscriber = editionNotificationSubscriber;
        }

        public async Task CreateEdition(NewEditionInput input)
        {
            if (input.Id == 0)
            {
                var newEdition = new CinotamEdition { DisplayName = input.DisplayName, Price = input.Price };
                await _editionManager.CreateAsync(newEdition);
                await CurrentUnitOfWork.SaveChangesAsync();
                await SetFeatureValues(newEdition, input.Features);

                await _editionNotificationsSender.SendNotificationEditionCreated(newEdition);

            }
            else
            {
                var edition = await _editionManager.GetByIdAsync(input.Id);

                var mapped = input.MapTo(edition);

                await _editionRepository.UpdateAsync(mapped);

                await SetFeatureValues(mapped, input.Features);
                await _editionNotificationsSender.SendNotificationEditionEdited(edition, await GetCurrentUserAsync());
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
                    Features = _customEditionManager.GetAllFeatures(),
                };
            }
            var edition = await _editionManager.GetByIdAsync(id.Value);

            var editionInput = edition.MapTo<NewEditionInput>();

            editionInput.Features = _customEditionManager.GetAllFeatures(edition.Id);

            return editionInput;
        }

        public async Task DeleteEdition(DeleteEditionInput input)
        {
            var edition = await _editionManager.FindByIdAsync(input.EditionId);
            await _editionManager.DeleteAsync(edition);

            await _editionNotificationsSender.SendNotificationEditionDeleted(edition);
        }
        public async Task SetEditionForTenant(int tenantId, int editionId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);

            if (tenant.EditionId != null)
            {
                var tenantPrevEdition = _editionRepository.Get(tenant.EditionId.Value);
                await _editionNotificationSubscriber.UnSubscribeTenantToEditionChanges(tenant, tenantPrevEdition, GetTenantOwner(tenant));

            }

            var edition = _editionRepository.Get(editionId);
            var editionFeatureValues = await _editionManager.GetFeatureValuesAsync(editionId);
            await _tenantManager.SetFeatureValuesAsync(tenant.Id, editionFeatureValues.ToArray());

            await _editionNotificationSubscriber.SubscribeTenantToEditionChanges(tenant, edition, GetTenantOwner(tenant));
        }

        private const string TenantOwnerUserName = "admin";
        private User GetTenantOwner(Tenant tenant)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant, AbpDataFilters.MayHaveTenant))
            {
                var owner = UserManager.Users.FirstOrDefault(a => a.UserName == TenantOwnerUserName && a.TenantId == tenant.Id);

                if (owner == null) throw new UserFriendlyException(L("TenantOwnerNotFound"));

                return owner;
            }
        }
    }
}
