(function () {
    "use strict";
    $ (document).ready(function () {

        var table = $("#usersTable").DataTable({
            "bServerSide": true,
            "bPaginate": true,
            "sPaginationType": "full_numbers", // And its type.
            "iDisplayLength": 10,
            "ajax": "/SysAdmin/Users/" + "LoadUsers",
            "autoWidth": true,
            "preDrawCallback": function () {
                // Initialize the responsive datatables helper once.
                if (!responsiveHelper_dt_basic) {
                    responsiveHelper_dt_basic = new ResponsiveDatatablesHelper($('#usersTable'), breakpointDefinition);
                }
            },
            "rowCallback": function (nRow) {
                responsiveHelper_dt_basic.createExpandIcon(nRow);
            },
            "drawCallback": function (oSettings) {
                responsiveHelper_dt_basic.respond();
            },
            language: window.dataTablesLang,
            //dataSrc: 'result.data',
            columnDefs: [
                {
                    className: "text-center",
                    "render": function (data, type, row) {
                        return " <a data-modal href='/SysAdmin/Users/CreateEditUser/" + row.Id + "' class='btn btn-default btn-xs' title='Editar usuario' ><i class='fa fa-edit'></i></a> <a data-modal href='/SysAdmin/Users/EditRoles/" + row.Id + "' class='btn btn-default btn-xs' title='Editar roles' ><i class='fa fa-lock'></i></a>";
                    },
                    "targets": 2
                },
        {
            "render": function (data, type, row) {
                return row.Name + " " + row.Surname + " (<strong>Usuario:</strong> " + row.UserName + ")";
            },
            "targets": 0
        }
            ],
            initComplete: function () {
                var id = $("#ActivatorUserId").val();
                console.log(id);
                if (id != 0) {
                    window.modalInstance.open("/SysAdmin/Users/EditRoles/" + id + "");

                }
            },
            columns: [

                {
                    "data": "Name"

                },
                { "data": "EmailAddress" },
                { "data": "Id" }
            ],

        });

        // ReSharper disable once Html.EventNotResolved
        document.addEventListener('modalClose', modalHandler);

        function modalHandler(event) {
            console.log(event);
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
    });
})();