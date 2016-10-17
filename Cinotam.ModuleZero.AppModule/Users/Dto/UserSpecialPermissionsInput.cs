using Cinotam.ModuleZero.AppModule.Roles.Dto;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Users.Dto
{
    public class UserSpecialPermissionsInput
    {
        public long? UserId { get; set; }
        public IEnumerable<AssignedPermission> AssignedPermissions { get; set; }
    }
}
