/// <reference path="datatables.responsiveConfigs.js" />
(function () {
    var columns = [
        {
            "data": "DisplayName"
        },
        { "data": "CreationTimeString" },
        { "data": "Id" }
    ];
    var columnDefs = [
        {
            "name": "CreationTime",
            "targets": "1"
        },
        {
            className: "text-center",
            "render": function (data, type, row) {
                if (!row.IsStatic) {
                    return " <a data-modal href='/SysAdmin/Roles/CreateEditRole/" +
                        row.Id +
                        "' class='btn btn-default btn-xs' title='" +
                        LSys("EditRole") +
                        "' ><i class='fa fa-edit'></i></a>";
                } else {
                    return " <a disabled class='btn btn-default btn-xs' title='" +
                        LSys("EditRole") +
                        "' ><i class='fa fa-edit'></i></a>";
                }

            },
            "targets": 2
        }
    ];
    var dataTableConfig = new DatatablesConfig({
        DisplayLength: 10,
        Url: "/SysAdmin/Roles/" + "LoadRoles",
        ColumnDefinitions: columnDefs,
        Columns: columns,
        OnInitComplete: function (){},
        Element: $("#rolesTable")
    });

    var rolesPage = {
        dataTableConfig: dataTableConfig,
        eventHandler: function (event) {
            switch (event.detail.info.modalType) {
                case "MODAL_ROLES_SET":
                    table.ajax.reload();
                    abp.notify.success(LSys("RoleAssigned"), LSys("Success"));
                    break;
                case "MODAL_ROLE_CREATED":
                    table.ajax.reload();
                    abp.notify.success(LSys("RoleEdited"), LSys("Success"));
                    break;
                case "MODAL_ROLE_DELETED":
                    table.ajax.reload();
                    abp.notify.warn(LSys("RoleDeleted"), LSys("Success"));
                    break;
                default:
                    console.log("Event unhandled");
            }
        }
    };

    var table = $("#rolesTable").DataTable(rolesPage.dataTableConfig);
    document.addEventListener('modalClose', rolesPage.eventHandler);
})();