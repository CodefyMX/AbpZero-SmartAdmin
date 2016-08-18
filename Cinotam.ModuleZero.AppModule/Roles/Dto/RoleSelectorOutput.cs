using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Roles.Dto
{
    public class RoleSelectorOutput
    {
        public long UserId { get; set; }
        public IEnumerable<RoleDto> RoleDtos { get; set; }
    }
}
