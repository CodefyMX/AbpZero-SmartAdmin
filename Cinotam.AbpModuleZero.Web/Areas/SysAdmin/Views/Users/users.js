(function () {
    "use strict";
    $(document).ready(function () {

        var _userAppService = abp.services.app.user;
        var isUserEditGranted = abp.auth.isGranted("Pages.SysAdmin.Users.Edit");
        var isRoleAssignGranted = abp.auth.isGranted("Pages.SysAdminRoles.Assign");
        var isUserDeleteGranted = abp.auth.isGranted("Pages.SysAdmin.Users.Delete");
        var isPermissionsGranted = abp.auth.isGranted("Pages.SysAdminPermissions");
        var $table = $("#usersTable");
        var $body = $("body");
        var $form = $("#createEditForm");

        var columns = [
                    {
                        "render": function (data, type, row) {

                            var editUserbtn = "";
                            var assignRoleRolebtn = "";
                            var deleteUserBtn = "";
                            var assignPermissions = "";
                            if (isUserEditGranted) {
                                editUserbtn = "<a data-modal href='/SysAdmin/Users/CreateEditUser/" +
                                    row.Id +
                                    "' class='btn btn-default btn-xs' title='Editar usuario' ><i class='fa fa-edit'></i></a>";
                            }
                            if (isRoleAssignGranted) {
                                assignRoleRolebtn = "  <a data-modal href='/SysAdmin/Users/EditRoles/" +
                                    row.Id +
                                    "' class='btn btn-default btn-xs' title='Editar roles' ><i class='fa fa-briefcase'></i></a>";
                            }
                            if (isUserDeleteGranted) {
                                deleteUserBtn = " <a data-id=" + row.Id + " data-full-name=" + row.FullName + " class='btn btn-default btn-xs js-delete-user' title='Delete user' ><i class='fa fa-trash'></i></a>";
                            }

                            if (isPermissionsGranted) {
                                assignPermissions = " <a class='btn btn-default btn-xs' href='/SysAdmin/Users/UserSpecialPermissions/" + row.Id + "' data-modal><i class='fa fa-lock'></i></a>";
                            }

                            return editUserbtn + assignRoleRolebtn + assignPermissions + deleteUserBtn;
                        },
                        "targets": 0
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
                        "targets": 5
                    }
        ];
        var table;
        var datatablesConfig = new DatatablesConfig({
            DisplayLength: 10,
            Url: "/SysAdmin/Users/" + "LoadUsers",
            ColumnDefinitions: columns,
            Columns: [
                {
                    "data": "Id"
                },
                    {
                        "data": "UserName"
                    },
                    {
                        "data": "Name"
                    },
                    {
                        "data": "Surname"
                    },
                    {
                        "data": "EmailAddress"
                    },
                {
                    "data": "IsLockoutEnabled"
                },
                    {
                        "data": "CreationTimeString",
                        name: "CreationTime"
                    },
                    {
                        "data": "LastLoginTimeString",
                        name: "LastLoginTime"
                    }
            ],
            OnInitComplete: function () {
            },
            Element: $table
        });
        var usersPage = {
            dataTableConfig: datatablesConfig,
            modalHandler: function modalHandler(event) {
                switch (event.detail.info.modalType) {
                    case "MODAL_USER_CREATED":
                        table.ajax.reload();
                        abp.notify.success(LSys("UserCreated"), LSys("Success"));
                        break;
                    case "MODAL_PERMISSIONS_SET":
                        abp.notify.success(LSys("PermissionsSet"), LSys("Success"));
                    default:
                        console.log("Event unhandled");
                }
            }
        }
        table = $table.DataTable(usersPage.dataTableConfig);
        document.addEventListener("modalClose", usersPage.modalHandler);

        $body.on("click", ".js-delete-user", function () {
            var fullName = $(this).data("full-name");
            var id = $(this).data("id");
            var confirmDelete = abp.utils.formatString(LSys("ConfirmDeleteUser"), fullName);
            abp.message.confirm(confirmDelete, LSys("ConfirmQuestion"), function (response) {
                if (response) {
                    abp.ui.setBusy($form, _userAppService.deleteUser(id).done(function () {
                        table.ajax.reload();
                        abp.notify.warn(LSys("UserDeleted"), LSys("Success"));
                    }));
                }
            });
        });

    });
})();