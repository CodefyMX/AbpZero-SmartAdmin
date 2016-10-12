namespace Cinotam.AbpModuleZero.Authorization
{
    public static class PermissionNames
    {
        /// <summary>
        /// Main Permission
        /// </summary>
        public const string Pages = "Pages";

        /// <summary>
        /// Tenants
        /// </summary>
        public const string PagesTenants = "Pages.Tenants";
        public const string PagesTenantsCreate = "Pages.Tenants.Create";
        public const string PagesTenantsEdit = "Pages.Tenants.Edit";
        public const string PagesTenantsDelete = "Pages.Tenants.Delete";
        public const string PagesTenantsAssignEdition = "Pages.Tenants.EditionAssign";
        public const string PagesTenantsAssignFeatures = "Pages.Tenants.FeatureAssign";

        /// <summary>
        /// OrganizationUnits
        /// </summary>
        public const string PagesSysAdminOrgUnit = "Pages.SysAdmin.OrganizationUnits";
        public const string PagesSysAdminOrgUnitCreate = "Pages.SysAdmin.OrganizationUnitsCreate";
        public const string PagesSysAdminOrgUnitEdit = "Pages.SysAdmin.OrganizationUnitsEdit";
        public const string PagesSysAdminOrgUnitDelete = "Pages.SysAdmin.OrganizationUnitsDelete";
        public const string PagesSysAdminOrgUnitAddUser = "Pages.SysAdmin.OrganizationUnitsAddUser";
        public const string PagesSysAdminOrgUnitRemoveUser = "Pages.SysAdmin.OrganizationUnitsRemoveUser";

        /// <summary>
        /// Dashboard
        /// </summary>
        public const string PagesDashboard = "Pages.DashBoard";

        /// <summary>
        /// Users
        /// </summary>
        public const string PagesSysAdminUsers = "Pages.SysAdmin.Users";
        public const string PagesSysAdminUsersCreate = "Pages.SysAdmin.Users.Create";
        public const string PagesSysAdminUsersEdit = "Pages.SysAdmin.Users.Edit";
        public const string PagesSysAdminUsersDelete = "Pages.SysAdmin.Users.Delete";


        /// <summary>
        /// Roles
        /// </summary>
        public const string PagesSysAdminRoles = "Pages.SysAdminRoles";
        public const string PagesSysAdminRolesCreate = "Pages.SysAdminRoles.Create";
        public const string PagesSysAdminRolesEdit = "Pages.SysAdminRoles.Edit";
        public const string PagesSysAdminRolesDelete = "Pages.SysAdminRoles.Delete";
        public const string PagesSysAdminRolesAssign = "Pages.SysAdminRoles.Assign";
        /// <summary>
        /// Permissions
        /// </summary>
        public const string PagesSysAdminPermissions = "Pages.SysAdminPermissions";
        /// <summary>
        /// Configuration
        /// </summary>
        public const string PagesSysAdminConfiguration = "Pages.SysAdminConfiguration";
        /// <summary>
        /// Languages
        /// </summary>
        public const string PagesSysAdminLanguages = "Pages.SysAdminLanguages";
        public const string PagesSysAdminLanguagesCreate = "Pages.SysAdminLanguages.Create";
        public const string PagesSysAdminLanguagesDelete = "Pages.SysAdminLanguages.Delete";
        public const string PagesSysAdminLanguagesChangeTexts = "Pages.SysAdminLanguages.ChangeTexts";
        /// <summary>
        /// AuditLogs
        /// </summary>
        public const string AuditLogs = "Pages.AuditLogs";
    }
}