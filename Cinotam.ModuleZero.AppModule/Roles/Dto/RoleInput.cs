using Abp.Application.Services.Dto;
using Cinotam.AbpModuleZero.Extensions;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Roles.Dto
{
    public class RoleInput : EntityDto
    {
        public RoleInput()
        {
            AssignedPermissions = new List<AssignedPermission>();
        }
        public string DisplayName { get; set; }
        public string RoleName => DisplayName.Sluggify();
        public bool Granted { get; set; }
        public IEnumerable<AssignedPermission> AssignedPermissions { get; set; }

    }
}