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
                        return "<a href='/SysAdmin/Languages/GetLanguageTexts?targetLang=" + row.Name + "' class='btn btn-default btn-xs' title='" + LSys("EditTexts") + "' ><i class='fa fa-edit'></i></a> <a href='/SysAdmin/Languages/GetLanguageTexts?targetLang=" + row.Name + "' class='btn btn-danger btn-xs disabled' title='" + LSys("DeleteLanguage") + "' ><i class='fa fa-times'></i></a>";
                    } else {
                        return "<a href='/SysAdmin/Languages/GetLanguageTexts?targetLang=" + row.Name + "' class='btn btn-default btn-xs' title='" + LSys("EditTexts") + "' ><i class='fa fa-edit'></i></a> <a data-name='" + row.DisplayName + "' data-code=" + row.Name + " class='btn btn-danger btn-xs js-delete-language' title='" + LSys("DeleteLanguage") + "' ><i class='fa fa-times'></i></a>";
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
                        return "<span class='label label-default'>" + LSys("Static") + "</span>";
                    } else {
                        return "<span class='label label-primary'>" + LSys("NoStatic") + "</span>";
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

    $('body').on('click', '.js-delete-language', function () {
        var code = $(this).data('code');
        var name = $(this).data('name');
        
        abp.message.confirm(abp.utils.formatString(LSys("DeleteLanguageMessage"), name), LSys("ConfirmQuestion"), function (response) {
            if (response) {
                abp.ui.setBusy('body', abp.services.app.language.deleteLanguage(code).done(function () {
                    table.ajax.reload();
                    abp.notify.warn("Lenguaje [" + name + "] eliminado", "¡Exito!");
                }));
            }
        });
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