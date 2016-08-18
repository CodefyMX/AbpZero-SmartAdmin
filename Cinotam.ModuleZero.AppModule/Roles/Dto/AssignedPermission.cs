using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Roles.Dto
{
    public class AssignedPermission
    {
        public AssignedPermission()
        {
            ChildPermissions = new List<AssignedPermission>();
        }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public bool Granted { get; set; }
        public List<AssignedPermission> ChildPermissions { get; set; }
        public string ParentPermission { get; set; }
    }
}