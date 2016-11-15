using Abp.Application.Services;
using Cinotam.FileManager.Service.AppService.Dto;
using System.Threading.Tasks;

namespace Cinotam.FileManager.Service.AppService
{
    public interface IFileManagerAppService : IApplicationService
    {
        Task<SavedFileResponse> SaveFile(SaveFileInput input);
        Task<SavedFileResponse> SaveFile(string base64string);

    }
}
