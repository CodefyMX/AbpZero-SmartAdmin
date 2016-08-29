
using Abp.BackgroundJobs;
using Abp.Dependency;
using Cinotam.FileManager.Files;
using Cinotam.FileManager.Files.Inputs;
using Cinotam.FileManager.FileTypes;
using Cinotam.FileManager.SharedTypes.Enums;
using Cinotam.ModuleZero.BackgroundTasks.BackgroundJobs.ImagePublisher.Dto;

namespace Cinotam.ModuleZero.BackgroundTasks.BackgroundJobs.ImagePublisher
{
    public class ProfilePictureCdnPublisher : BackgroundJob<PublisherInputDto>, ITransientDependency
    {

        private readonly IFileStoreManager _fileStoreManager;

        public ProfilePictureCdnPublisher(IFileStoreManager fileStoreManager)
        {
            _fileStoreManager = fileStoreManager;
        }


        public override void Execute(PublisherInputDto args)
        {
            _fileStoreManager.SaveFileToCloudServiceFromString(new FileSaveFromStringInput()
            {
                CreateUniqueName = false,
                FileType = ValidFileTypes.Image,
                ImageEditOptions = new ImageEditOptionsRequest()
                {
                    Height = 120,
                    Width = 120,
                    TransFormationType = TransformationsTypes.ImageWithSize

                },
                File = args.File,
                SpecialFolder = args.UserName
            });
        }
    }
}
