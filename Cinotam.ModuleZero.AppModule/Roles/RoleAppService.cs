using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.AutoMapper;
using Abp.Localization;
using Cinotam.AbpModuleZero.Authorization;
using Cinotam.AbpModuleZero.Authorization.Roles;
using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.ModuleZero.AppModule.Roles.Dto;
using Cinotam.ModuleZero.Notifications.RolesAppNotifications.Sender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cinotam.ModuleZero.AppModule.Roles
{
    [AbpAuthorize(PermissionNames.PagesSysAdminRoles)]
    public class RoleAppService : CinotamModuleZeroAppServiceBase, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;
        private readonly IRolesAppNotificationsSender _rolesAppNotificationsSender;
        public RoleAppService(RoleManager roleManager, IPermissionManager permissionManager, IRolesAppNotificationsSender rolesAppNotificationsSender)
        {
            _roleManager = roleManager;
            _permissionManager = permissionManager;
            _rolesAppNotificationsSender = rolesAppNotificationsSender;
        }

        public async Task DeleteRole(int roleId)
        {
            var role = await _roleManager.GetRoleByIdAsync(roleId);
            await _roleManager.DeleteAsync(role);
        }
        public async Task UpdateRolePermissions(UpdateRolePermissionsInput input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.RoleId);
            var grantedPermissions = _permissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissionNames.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }

        public async Task CreateEditRole(RoleInput input)
        {
            var permissions =
                    input.AssignedPermissions.Select(assignedPermission => new Permission(assignedPermission.Name))
                        .ToList();
            if (input.Id == 0)
            {
                var role = new Role(AbpSession.TenantId, input.RoleName, input.DisplayName);

                await _roleManager.CreateAsync(role);

                await CurrentUnitOfWork.SaveChangesAsync();

                await _roleManager.SetGrantedPermissionsAsync(role, permissions);
                await _rolesAppNotificationsSender.SendRoleCreatedNotification((await GetCurrentUserAsync()), role);
            }
            else
            {
                var role = await _roleManager.GetRoleByIdAsync(input.Id);
                role.Name = input.RoleName;
                role.DisplayName = input.DisplayName;
                role.TenantId = AbpSession.TenantId;
                await _roleManager.UpdateAsync(role);
                await CurrentUnitOfWork.SaveChangesAsync();
                await _roleManager.SetGrantedPermissionsAsync(role, permissions);
                await _rolesAppNotificationsSender.SendRoleEditedNotification((await GetCurrentUserAsync()), role);
            }

        }
        public ReturnModel<RoleDto> GetRolesForTable(RequestModel<object> input)
        {
            var query = _roleManager.Roles.AsQueryable();
            int totalCount;

            var search = new List<Expression<Func<Role, string>>>() { a => a.DisplayName, a => a.Name };


            var filterByLength = GenerateTableModel(input, query, search, "Name", out totalCount);
            return new ReturnModel<RoleDto>()
            {
                data = filterByLength.Select(a => a.MapTo<RoleDto>()).ToArray(),
                draw = input.draw,
                iTotalDisplayRecords = totalCount,
                recordsTotal = totalCount,
                iTotalRecords = query.Count(),
                recordsFiltered = filterByLength.Count,
                length = input.length
            };
        }

        public async Task<RoleInput> GetRoleForEdit(int? id)
        {
            var allPermissions = _permissionManager.GetAllPermissions().Where(a => a.Parent == null).ToList();
            if (id != null)
            {
                var role = await _roleManager.GetRoleByIdAsync(id.Value);
                if (role == null) return new RoleInput();
                var assignedPermissions = CheckPermissions(allPermissions, role.Permissions.ToList());
                return new RoleInput()
                {
                    AssignedPermissions = assignedPermissions,
                    DisplayName = role.DisplayName,
                    Id = role.Id
                };
            }

            return new RoleInput()
            {
                AssignedPermissions = CheckPermissions(allPermissions,
                new List<RolePermissionSetting>())
            };
        }
        private IEnumerable<AssignedPermission> CheckPermissions(IEnumerable<Permission> allPermissions, ICollection<RolePermissionSetting> rolePermissions)
        {
            var permissionsFound = new List<AssignedPermission>();
            foreach (var permission in allPermissions)
            {
                AddPermission(permissionsFound, rolePermissions, permission, rolePermissions.Any(a => a.Name == permission.Name));
            }
            return permissionsFound;
        }
        private void AddPermission(ICollection<AssignedPermission> permissionsFound, ICollection<RolePermissionSetting> rolePermissions, Permission allPermission, bool granted)
        {

            var childPermissions = new List<AssignedPermission>();
            var permission = new AssignedPermission()
            {
                DisplayName = allPermission.DisplayName.Localize(new LocalizationContext(LocalizationManager)),
                Granted = granted,
                Name = allPermission.Name,
            };
            if (allPermission.Children.Any())
            {
                foreach (var childPermission in allPermission.Children)
                {
                    AddPermission(childPermissions, rolePermissions, childPermission, rolePermissions.Any(a => a.Name == childPermission.Name));
                }

                permission.ChildPermissions.AddRange(childPermissions);
            }

            permissionsFound.Add(permission);
        }


    }
}