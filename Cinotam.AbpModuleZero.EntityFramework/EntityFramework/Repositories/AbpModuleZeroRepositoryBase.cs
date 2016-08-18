using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Cinotam.AbpModuleZero.EntityFramework.Repositories
{
    public abstract class AbpModuleZeroRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<AbpModuleZeroDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected AbpModuleZeroRepositoryBase(IDbContextProvider<AbpModuleZeroDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class AbpModuleZeroRepositoryBase<TEntity> : AbpModuleZeroRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected AbpModuleZeroRepositoryBase(IDbContextProvider<AbpModuleZeroDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
