namespace Cinotam.AbpModuleZero.Tools.AngularHelpers
{
    public class AngularCustomObj
    {
        public class AngularMenuItem
        {
            public AngularMenuItem(bool hasPermission,
                string permissionName,
                string stateName,
                string uiUrl,
                string templateUrl,
                bool isOtherApp)
            {
                HasPermission = hasPermission;
                PermissionName = permissionName;
                StateName = stateName;
                UiUrl = uiUrl;
                TemplateUrl = templateUrl;
                IsOtherApp = isOtherApp;
            }
            protected AngularMenuItem() { }
            public bool IsOtherApp { get; set; }

            public string TemplateUrl { get; set; }

            public string UiUrl { get; set; }

            public string StateName { get; set; }

            public string PermissionName { get; set; }

            public bool HasPermission { get; set; }

        }
    }
}
