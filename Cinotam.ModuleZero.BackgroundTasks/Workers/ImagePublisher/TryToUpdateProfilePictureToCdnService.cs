using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Cinotam.AbpModuleZero.Users;
using Cinotam.FileManager.Files;
using Cinotam.FileManager.Files.Inputs;
using System;
using System.IO;
using System.Web.Hosting;
using Abp.Threading;

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
                    var result = AsyncHelper.RunSync(() => _fileStoreManager.SaveFile(new FileSaveFromStringInput()
                    {
                        CreateUniqueName = false,
                        FilePath = GetAbsolutePath(user.ProfilePicture),
                        Properties =
                        {
                            ["Width"] = 120,
                            ["Height"] = 120,
                            ["TransformationType"] = 2
                        },
                        SpecialFolder = user.UserName
                    }, true));
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
