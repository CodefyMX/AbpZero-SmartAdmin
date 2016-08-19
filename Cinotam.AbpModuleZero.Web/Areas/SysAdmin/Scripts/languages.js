/// <reference path="datatables.responsiveConfigs.js" />

(function () {
    "use strict";
    window.table = $("#languagesTable").DataTable({
        "bServerSide": true,
        "bPaginate": true,
        "sPaginationType": "full_numbers", // And its type.
        "iDisplayLength": 10,
        "ajax": "/SysAdmin/Languages/" + "LoadLanguages",
        "autoWidth": true,
        "preDrawCallback": function () {
            // Initialize the responsive datatables helper once.
            if (!responsiveHelper_dt_languages) {
                responsiveHelper_dt_languages = new ResponsiveDatatablesHelper($('#languagesTable'), breakpointDefinition);
            }
        },
        "rowCallback": function (nRow) {
            responsiveHelper_dt_languages.createExpandIcon(nRow);
        },
        "drawCallback": function (oSettings) {
            responsiveHelper_dt_languages.respond();
        },
        language: window.dataTablesLang,
        //dataSrc: 'result.data',
        columnDefs: [
            {
                className: "text-center",
                "render": function (data, type, row) {
                    return " <a data-modal href='/SysAdmin/Languages/ChangeTexts/" + row.Id + "' class='btn btn-default btn-sm' title='Editar textos' ><i class='fa fa-edit'></i></a>";
                },
                "targets": 2
            },
            {
                "name": "CreationTime",
                "targets": 1
            },
            {
                "render": function (data, type, row) {
                    return "<i class="+row.Icon+"></i> " +row.Name;
                },
                "targets": 0
            }
        ],
        columns: [
            {
                "data": "Name"
            },
            { "data": "CreationTimeString" }
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