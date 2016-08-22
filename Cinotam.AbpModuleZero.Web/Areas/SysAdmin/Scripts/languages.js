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
                    return " <a href='/SysAdmin/Languages/GetLanguageTexts?targetLang=" + row.Name + "' class='btn btn-default btn-sm' title='Editar textos' ><i class='fa fa-edit'></i></a>";
                },
                "targets": 2
            },
            {
                "name": "CreationTime",
                "targets": 1
            },
            {
                "render": function (data, type, row) {
                    return "<i class="+row.Icon+"></i> " +row.DisplayName;
                },
                "targets": 0
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