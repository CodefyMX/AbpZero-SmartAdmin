using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Roles.Dto
{
    public class RoleSelectorOutput
    {
        public long UserId { get; set; }
        public string LastModifier { get; set; }
        public string FullName { get; set; }
        public IEnumerable<RoleDto> RoleDtos { get; set; }
    }
}
