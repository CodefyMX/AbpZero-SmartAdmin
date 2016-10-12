/// <reference path="datatables.responsiveConfigs.js" />
/// <reference path="~/Areas/SysAdmin/Scripts/Layout/localizationText.js" />
/// <reference path="~/Areas/SysAdmin/Scripts/datatables.config.js" />
/// <reference path="~/Abp/Framework/scripts/abp.js" />
(function () {

    var rolePageGranted = abp.auth.isGranted("Pages.SysAdminRoles");
    var roleEditGranted = abp.auth.isGranted("Pages.SysAdminRoles.Edit");
    var roleDeleteGranted = abp.auth.isGranted("Pages.SysAdminRoles.Delete");
    $(document)
        .ready(function () {

            var _roleAppService = abp.services.app.role;
            var $body = $("body");
            var $createEditRoleElement = $("#createEditRole");

            var $table = $("#rolesTable");

            var columns = [
                { "data": "Id" },
                {
                    "data": "DisplayName"
                },
                { "data": "CreationTimeString" }

            ];
            var columnDefs = [
                {
                    "name": "CreationTime",
                    "targets": 2
                },
                {

                    "render": function (data, type, row) {
                        var roleDeleteBtn = "";
                        if (roleDeleteGranted) {
                            roleDeleteBtn = " <a data-role-name=" + row.DisplayName + " data-id=" + row.Id + " class='btn btn-default btn-xs js-delete-role' title='" +
                                LSys("EditRole") +
                                "' ><i class='fa fa-trash'></i></a>";
                        }

                        if (roleEditGranted) {
                            if (!row.IsStatic) {
                                return " <a data-modal href='/SysAdmin/Roles/CreateEditRole/" +
                                    row.Id +
                                    "' class='btn btn-default btn-xs' title='" +
                                    LSys("EditRole") +
                                    "' ><i class='fa fa-edit'></i></a>" + roleDeleteBtn;
                            } else {
                                return " <a data-modal href='/SysAdmin/Roles/CreateEditRole/" +
                                    row.Id +
                                    "' class='btn btn-default btn-xs' title='" +
                                    LSys("EditRole") +
                                    "' ><i class='fa fa-edit'></i></a>";
                            }
                        }

                        return "";

                    },
                    "targets": 0
                }
            ];
            var dataTableConfig = new DatatablesConfig({
                DisplayLength: 10,
                Url: "/SysAdmin/Roles/" + "LoadRoles",
                ColumnDefinitions: columnDefs,
                Columns: columns,
                OnInitComplete: function () { },
                Element: $("#rolesTable")
            });
            var table;
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
                        default:
                            console.log("Event unhandled");
                    }
                }
            };

            $body.on("click", ".js-delete-role", function () {
                var id = $(this).data("id");
                var roleName = $(this).data("role-name");
                var message = abp.utils.formatString(LSys("RoleDeleteMessage"), roleName);

                abp.message.confirm(message, LSys("ConfirmQuestion"), function (response) {
                    if (response) {
                        abp.ui.setBusy($createEditRoleElement, _roleAppService.deleteRole(id).done(function () {
                            table.ajax.reload();
                            abp.notify.warn(LSys("RoleDeleted"), LSys("Success"));
                        }));
                    }
                });
            });
            if (rolePageGranted) {

                table = $table.DataTable(rolesPage.dataTableConfig);
            }

            document.addEventListener('modalClose', rolesPage.eventHandler);
        });


})();