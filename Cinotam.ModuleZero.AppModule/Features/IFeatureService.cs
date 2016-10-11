using Abp.Application.Services;
using Cinotam.ModuleZero.AppModule.Features.Dto;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Features
{
    public interface IFeatureService : IApplicationService
    {
        Task CreateEdition(NewEditionInput input);
        Task SetFeatureValue(FeatureDto input);
        EditionsOutput GetEditions();
        Task<NewEditionInput> GetEditionForEdit(int? id);
        Task DeleteEdition(DeleteEditionInput input);
    }
}
