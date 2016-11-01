using Abp.Domain.Services;
using Cinotam.ModuleZero.AppModule.Features.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Features.FeatureManager
{
    /// <summary>
    /// Some shared features for editions and features
    /// </summary>
    public interface ICustomEditionManager : IDomainService
    {
        List<FeatureDto> GetAllFeatures(int? id = null);
        Task<List<FeatureDto>> GetAllFeatures(int editionId, int tenantId);
    }
}
