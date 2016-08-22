/// <reference path="datatables.responsiveConfigs.js" />

(function () {
    "use strict";
    var table = $("#languagesTable").DataTable({
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
                    if (row.IsStatic) {
                        return "<a href='/SysAdmin/Languages/GetLanguageTexts?targetLang=" + row.Name + "' class='btn btn-default btn-xs' title='Editar textos' ><i class='fa fa-edit'></i></a> <a href='/SysAdmin/Languages/GetLanguageTexts?targetLang=" + row.Name + "' class='btn btn-danger btn-xs disabled' title='Eliminar lenguaje' ><i class='fa fa-times'></i></a>";
                    } else {
                        return "<a href='/SysAdmin/Languages/GetLanguageTexts?targetLang=" + row.Name + "' class='btn btn-default btn-xs' title='Editar textos' ><i class='fa fa-edit'></i></a> <a href='/SysAdmin/Languages/GetLanguageTexts?targetLang=" + row.Name + "' class='btn btn-danger btn-xs' title='Eliminar lenguaje' ><i class='fa fa-times'></i></a>";
                    }
                },
                "targets": 3
            },
            {
                "name": "CreationTime",
                "targets": 1
            },
            {
                "render": function (data, type, row) {
                    return "<i class=" + row.Icon + "></i> " + row.DisplayName;
                },
                "targets": 0
            },
            {
                "render": function (data, type, row) {
                    if (row.IsStatic) {
                        return "<span class='label label-default'>Estatico</span>";
                    } else {
                        return "<span class='label label-primary'>Definido por el usuario</span>";
                    }
                },
                "targets": 2
            }

        ],
        columns: [
            {
                "data": "DisplayName"
            },
            { "data": "CreationTimeString" }
        ]
    });



    document.addEventListener('modalClose', modalHandler);
    function modalHandler(event) {
        console.log(event);
        switch (event.detail.info.modalType) {
            case "LANGUAGE_CREATED":
                table.ajax.reload();
                abp.notify.success("Lenguaje creado", "¡Exito!");
                break;
            default:
                console.log("Event unhandled");
        }
    }

})();