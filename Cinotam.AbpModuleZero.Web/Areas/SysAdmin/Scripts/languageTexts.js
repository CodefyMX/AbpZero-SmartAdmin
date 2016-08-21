(function () {
    "use strict";

    $(document).ready(function() {
        var source = $("#Source").val();
        var targetLang = $("#SelectedTargetLanguage").val();
        var sourceLang = $("#SelectedSourceLanguage").val();
        var table = $("#languageTextsTable").DataTable({
            "bServerSide": true,
            "bPaginate": true,
            "sPaginationType": "full_numbers", // And its type.
            "iDisplayLength": 10,
            "ajax": "/SysAdmin/Languages/" + "GetLanguageTextsForTable?Source=" + source + "&TargetLang=" + targetLang + "&SourceLang=" + sourceLang,
            "autoWidth": true,
            "preDrawCallback": function () {
                // Initialize the responsive datatables helper once.
                if (!responsiveHelper_dt_languages) {
                    responsiveHelper_dt_languages = new ResponsiveDatatablesHelper($('#languageTextsTable'), breakpointDefinition);
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
                        return " <a  class='btn btn-default btn-sm' title='Editar texto' ><i class='fa fa-edit'></i></a>";
                    },
                    "targets": 3
                }
            ],
            columns: [
                {
                    "data": "Key"
                },
                { "data": "SourceValue" },
                { "data": "TargetValue" }
            ]
        });
    });


})();