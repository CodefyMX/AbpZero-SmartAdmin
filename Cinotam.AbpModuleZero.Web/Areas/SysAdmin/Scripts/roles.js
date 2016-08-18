/// <reference path="datatables.responsiveConfigs.js" />

(function () {
    "use strict";
    window.table = $("#rolesTable").DataTable({
        "bServerSide": true,
        "bPaginate": true,
        "sPaginationType": "full_numbers", // And its type.
        "iDisplayLength": 10,
        "ajax": "/SysAdmin/Roles/" + "LoadRoles",
        "autoWidth": true,
        "preDrawCallback": function () {
            // Initialize the responsive datatables helper once.
            if (!responsiveHelper_dt_roles) {
                responsiveHelper_dt_roles = new ResponsiveDatatablesHelper($('#rolesTable'), breakpointDefinition);
            }
        },
        "rowCallback": function (nRow) {
            responsiveHelper_dt_roles.createExpandIcon(nRow);
        },
        "drawCallback": function (oSettings) {
            responsiveHelper_dt_roles.respond();
        },
        language: window.dataTablesLang,
        //dataSrc: 'result.data',
        columnDefs: [
            {
                "name": "CreationTime",
                "targets":"1"
            },
            {
                className: "text-center",
                "render": function (data, type, row) {
                    if (!row.IsStatic) {
                        return " <a data-modal href='/SysAdmin/Roles/CreateEditRole/" + row.Id + "' class='btn btn-default btn-sm' title='Editar Rol' ><i class='fa fa-edit'></i></a>";
                    } else {
                        return " <a disabled class='btn btn-default btn-sm' title='Editar rol' ><i class='fa fa-edit'></i></a>";
                    }
                    
                },
                "targets": 2
            }
        ],
        columns: [

            {
                "data": "DisplayName"
            },
            { "data": "CreationTimeString" },
            { "data": "Id" }
        ]
    });



    document.addEventListener('modalClose', modalHandler);
    function modalHandler(event) {
        console.log(event);
        switch (event.detail.info) {
            case "MODAL_ROLES_SET":
                table.ajax.reload();
                abp.notify.success("Roles asignados", "¡Exito!");
                break;
            case "MODAL_ROLE_CREATED":
                table.ajax.reload();
                abp.notify.success("Rol creado/editado", "¡Exito!");
                break;
            case "MODAL_ROLE_DELETED":
                table.ajax.reload();
                abp.notify.warn("Rol eliminado", "¡Exito!");
                break;
            default:
                console.log("Event unhandled");
        }
    }

})();