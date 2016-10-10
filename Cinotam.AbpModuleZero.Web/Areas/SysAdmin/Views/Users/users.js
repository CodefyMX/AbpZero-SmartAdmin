(function () {
    "use strict";

    $(document).ready(function () {

        var columns = [
                    {
                        className: "text-center",
                        "render": function (data, type, row) {
                            return " <a data-modal href='/SysAdmin/Users/CreateEditUser/" + row.Id + "' class='btn btn-default btn-xs' title='Editar usuario' ><i class='fa fa-edit'></i></a> <a data-modal href='/SysAdmin/Users/EditRoles/" + row.Id + "' class='btn btn-default btn-xs' title='Editar roles' ><i class='fa fa-lock'></i></a>";
                        },
                        "targets": 3
                    },
                    {
                        className: "text-center",
                        "render": function (data, type, row) {
                            if (row.IsLockoutEnabled) {
                                return LSys("Locked");
                            } else {
                                return LSys("Unlocked");
                            }
                        },
                        "targets": 2
                    },
            {
                "render": function (data, type, row) {
                    return row.Name + " " + row.Surname + " (<strong>Usuario:</strong> " + row.UserName + ")";
                },
                "targets": 0
            }
        ];

        var datatablesConfig = new DatatablesConfig({
            DisplayLength: 10,
            Url: "/SysAdmin/Users/" + "LoadUsers",
            ColumnDefinitions: columns,
            Columns: [

                    {
                        "data": "Name"
                    },
                    {
                        "data": "EmailAddress"
                    }
            ],
            OnInitComplete: function () {
                var id = $("#ActivatorUserId").val();
                console.log(id);
                if (id != 0) {
                    window.modalInstance.open("/SysAdmin/Users/EditRoles/" + id + "");

                }
            },
            Element: $("#usersTable")
        });
        var usersPage = {
            dataTableConfig: datatablesConfig,
            modalHandler: function modalHandler(event) {
                switch (event.detail.info.modalType) {
                    case "MODAL_USER_CREATED":
                        table.ajax.reload();
                        abp.notify.success(LSys("UserCreated"), LSys("Success"));
                        break;
                    case "MODAL_USER_DELETED":
                        table.ajax.reload();
                        abp.notify.warn(LSys("UserDeleted"), LSys("Success"));
                        break;
                    default:
                        console.log("Event unhandled");
                }
            }
        }
        var table = $("#usersTable").DataTable(usersPage.dataTableConfig);
        document.addEventListener('modalClose', usersPage.modalHandler);
    });
})();