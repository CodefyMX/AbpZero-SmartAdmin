(function () {
    "use strict";
    var modalType = "MODAL_USER_ADDED";

    $(document).ready(function () {

        var $body = $("body");
        var $table = $("#usersTable");
        var orgUnitId = $("#OrgUnitId").val();

        var columns = [
                    {
                        className: "text-center",
                        "render": function (data, type, row) {
                            return " <a data-org-id=" + orgUnitId + " data-user-id=" + row.Id + " class='btn btn-default btn-xs js-add-user' title='"+LSys("AddUser")+"' ><i class='fa fa-check'></i></a>";
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
            Element: $table
        });
        var usersPage = {
            dataTableConfig: datatablesConfig

        }

       $body
            .on("click",
                ".js-add-user",
                function () {
                    var $self = $(this);
                    var userId = $self.data("user-id");
                    var orgUnitId = $self.data("org-id");

                    abp.ui.setBusy($table, abp.services.app.organizationUnits.addUserToOrgUnit({
                        UserId: userId,
                        OrgUnitId: orgUnitId
                    }).done(function () {
                        window.modalInstance.close({}, modalType);
                        
                    }));
                });


        var table =$table.DataTable(usersPage.dataTableConfig);
    });
})();