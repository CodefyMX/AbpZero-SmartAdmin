(function () {
    "use strict";

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
                    return " <a data-modal href='/SysAdmin/Users/CreateEditUser/" + row.Id + "' class='btn btn-default btn-sm' title='Editar usuario' ><i class='fa fa-edit'></i></a> <a data-modal href='/SysAdmin/Users/EditRoles/" + row.Id + "' class='btn btn-default btn-sm' title='Editar roles' ><i class='fa fa-lock'></i></a>";
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
        columns: [

            {
                "data": "Name"

            },
            { "data": "EmailAddress" },
            { "data": "Id" }
        ]
    });
    document.addEventListener('modalClose', modalHandler);
    function modalHandler(event) {
        console.log(event);
        switch (event.detail.info.modalType) {
            case "MODAL_USER_CREATED":
                table.ajax.reload();
                abp.notify.success("Usuario creado/editado", "¡Exito!");
                break;
            case "MODAL_USER_DELETED":
                table.ajax.reload();
                abp.notify.warn("Usuario eliminado", "¡Exito!");
                break;
            default:
                console.log("Event unhandled");
        }
    }
})();