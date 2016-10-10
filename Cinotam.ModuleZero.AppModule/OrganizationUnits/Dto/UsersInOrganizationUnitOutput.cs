using Cinotam.ModuleZero.AppModule.Users.Dto;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.OrganizationUnits.Dto
{
    public class UsersInOrganizationUnitOutput
    {
        public long OrgUnitId { get; set; }
        public List<UserListDto> Users { get; set; }
    }

}
