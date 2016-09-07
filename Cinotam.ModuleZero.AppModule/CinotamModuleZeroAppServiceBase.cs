using Abp.Application.Services;
using Abp.Configuration;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Cinotam.AbpModuleZero;
using Cinotam.AbpModuleZero.Authorization.Roles;
using Cinotam.AbpModuleZero.MultiTenancy;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.ReflectionHelpers;
using Cinotam.AbpModuleZero.Users;
using Microsoft.AspNet.Identity;
using NinjaNye.SearchExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule
{
    public class CinotamModuleZeroAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        public RoleManager RoleManager { get; set; }
        public SettingManager SettingsManager { get; set; }

        protected CinotamModuleZeroAppServiceBase()
        {
            LocalizationSourceName = AbpModuleZeroConsts.LocalizationSourceName;
        }

        public async Task<string> GetLastEditedForName(long? lastEditedForId)
        {
            return lastEditedForId != null ? (await UserManager.GetUserByIdAsync(lastEditedForId.Value)).FullName : "";
        }
        private static IQueryable<T> GetOrderedQuery<T>(IQueryable<T> elements, RequestModel<object> requestModel)
        {
            return requestModel.PropOrd.ToUpper() == "ASC" ? elements.OrderBy(requestModel.PropToSort) : elements.OrderByDescending(requestModel.PropToSort);
        }

        /// <summary>
        /// Generates a readyforuse datatable.js model
        /// </summary>
        /// <typeparam name="TQ"></typeparam>
        /// <param name="request">Request model from datatables.js</param>
        /// <param name="queryable">Query of the entity</param>
        /// <param name="defaultOrderableProp">A default orderable property (if the method doesnt find the requested prop use this has callback)</param>
        /// <param name="totalCount"></param>
        /// <returns>ListOfTQ</returns>
        public List<TQ> GenerateTableModel<TQ>(RequestModel<object> request, IQueryable<TQ> queryable, string defaultOrderableProp, out int totalCount)
        {
            var pageIndex = request.start;
            var pageSize = request.length;
            var searchString = request.search["value"].ToUpper();
            totalCount = queryable.Count();
            if (pageSize == -1)
            {
                pageIndex = 0;
                request.length = totalCount;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                queryable = queryable.Where(request.PropToSearch, searchString);
            }
            queryable = !string.IsNullOrEmpty(request.PropToSort) ? GetOrderedQuery(queryable, request) : queryable.OrderBy(defaultOrderableProp);
            var filteredByLength = queryable.Skip(pageIndex).Take(request.length).ToList();
            return filteredByLength;
        }
        public List<TQ> GenerateTableModel<TQ>(RequestModel<object> request, IQueryable<TQ> queryable, List<Expression<Func<TQ, string>>> expression, string defaultOrderableProp, out int totalCount)
        {
            var pageIndex = request.start;
            var pageSize = request.length;
            totalCount = queryable.Count();
            if (pageSize == -1)
            {
                pageIndex = 0;
                request.length = totalCount;
            }

            var searchString = request.search["value"].ToUpper();

            queryable = queryable.Search(expression.ToArray()).Containing(searchString);

            queryable = !string.IsNullOrEmpty(request.PropToSort) ? GetOrderedQuery(queryable, request) : queryable.OrderBy(defaultOrderableProp);
            var filteredByLength = queryable.Skip(pageIndex).Take(request.length);
            return filteredByLength.ToList();
        }

        protected virtual Task<User> GetCurrentUserAsync()
        {
            var user = UserManager.FindByIdAsync(AbpSession.GetUserId());
            if (user == null)
            {
                throw new ApplicationException("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
