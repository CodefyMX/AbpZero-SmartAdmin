using Abp.Application.Services;
using Abp.Domain.Services;

namespace Cinotam.ModuleZero.AppModule.MultiTenancy.MultiTenancyHelper
{
                            //We dont want this to be available in the js services
    public interface IMultiTenancyHelper : IDomainService
    {
        /// <summary>
        /// Sets the current tenancy and returns its unique name
        /// </summary>
        /// <returns></returns>
        string SetCurrentTenancy(string absoluteUrl);
    }
}
