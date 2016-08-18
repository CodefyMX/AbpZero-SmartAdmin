using Abp.Application.Services;
using Cinotam.ModuleZero.AppModule.Sessions.Dto;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
