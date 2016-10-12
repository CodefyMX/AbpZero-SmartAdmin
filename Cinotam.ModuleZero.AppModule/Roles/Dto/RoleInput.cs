using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Cinotam.AbpModuleZero.Authorization.Roles;
using Cinotam.AbpModuleZero.Extensions;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Roles.Dto
{
    [AutoMap(typeof(Role))]
    public class RoleInput : EntityDto
    {
        public RoleInput()
        {
            AssignedPermissions = new List<AssignedPermission>();
        }
        public string DisplayName { get; set; }
        public string Name => DisplayName.Sluggify();
        public bool Granted { get; set; }
        public bool IsDefault { get; set; }
        public IEnumerable<AssignedPermission> AssignedPermissions { get; set; }

    }
}