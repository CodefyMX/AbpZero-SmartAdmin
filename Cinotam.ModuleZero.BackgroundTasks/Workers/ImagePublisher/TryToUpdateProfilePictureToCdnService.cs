using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Cinotam.AbpModuleZero.Users;
using Cinotam.FileManager.Files;
using Cinotam.FileManager.Files.Inputs;
using Cinotam.FileManager.FileTypes;
using Cinotam.FileManager.SharedTypes.Enums;
using System;
using System.IO;
using System.Web.Hosting;

namespace Cinotam.ModuleZero.BackgroundTasks.Workers.ImagePublisher
{
    public class TryToUpdateProfilePictureToCdnService : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IFileStoreManager _fileStoreManager;
        private readonly IRepository<User, long> _usersRepository;

        public TryToUpdateProfilePictureToCdnService(AbpTimer timer, IFileStoreManager fileStoreManager,
            IRepository<User, long> usersRepository) : base(timer)
        {
            _fileStoreManager = fileStoreManager;
            _usersRepository = usersRepository;

            Timer.Period = 10000;
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            var usersWithNoCdn = _usersRepository.GetAllList(a => a.IsPictureOnCdn == false);
            foreach (var user in usersWithNoCdn)
            {
                try
                {
                    var profilePictureHolder = user.ProfilePicture;
                    if (string.IsNullOrEmpty(user.ProfilePicture)) continue;
                    var result = _fileStoreManager.SaveFileToCloudServiceFromString(new FileSaveFromStringInput()
                    {
                        CreateUniqueName = false,
                        File = GetAbsolutePath(user.ProfilePicture),
                        FileType = ValidFileTypes.Image,
                        ImageEditOptions = new ImageEditOptionsRequest()
                        {
                            Height = 120,
                            Width = 120,
                            TransFormationType = TransformationsTypes.ImageWithSize,

                        },
                        SpecialFolder = user.UserName
                    });
                    if (!result.WasStoredInCloud) continue;
                    user.ProfilePicture = result.Url;
                    user.IsPictureOnCdn = true;
                    RemoveTempFile(profilePictureHolder);
                }
                catch (Exception)
                {
                    //
                }


            }
            CurrentUnitOfWork.SaveChanges();
        }

        private string GetAbsolutePath(string virtualPath)
        {
            var path = HostingEnvironment.MapPath(virtualPath);
            return path;
        }

        private void RemoveTempFile(string virtualPath)
        {
            var path = HostingEnvironment.MapPath(virtualPath);

            if (path == null) return;
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
