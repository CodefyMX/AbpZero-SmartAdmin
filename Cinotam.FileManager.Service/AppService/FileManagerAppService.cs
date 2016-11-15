using Cinotam.FileManager.Contracts;
using Cinotam.FileManager.Files;
using Cinotam.FileManager.Service.AppService.Dto;
using System.Threading.Tasks;

namespace Cinotam.FileManager.Service.AppService
{
    public class FileManagerAppService : IFileManagerAppService
    {
        private readonly IFileStoreManager _fileStoreManager;

        public FileManagerAppService(IFileStoreManager fileStoreManager)
        {
            _fileStoreManager = fileStoreManager;
        }

        public async Task<SavedFileResponse> SaveFile(SaveFileInput input)
        {
            var result = await _fileStoreManager.SaveFile(new FileManagerServiceInput()
            {
                CreateUniqueName = false,
                File = input.File,
                SpecialFolder = input.SaveFolder,
                VirtualFolder = "/Content/Attachments/",
                Properties = input.Properties
            }, CinotamFileManagerService.UseCdn);

            return new SavedFileResponse()
            {
                FileName = input.File.FileName,
                Url = result.Url,
                StoredInCloud = result.WasStoredInCloud
            };
        }

        public Task<SavedFileResponse> SaveFile(string base64string)
        {
            throw new System.NotImplementedException();
        }
    }
}
