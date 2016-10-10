(function () {
    "use strict";

    $(document).ready(function () {

        drawBreadCrumb([LSys("Language"), LSys("Texts")]);

        var source = $("#Source").val();
        var targetLang = $("#SelectedTargetLanguage").val();
        var sourceLang = $("#SelectedSourceLanguage").val();

        var table = $("#languageTextsTable").DataTable({
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
                        return " <a data-current='"+row.TargetValue+"' data-source='" + $("#Source").val() + "' data-lang='" + $("#SelectedTargetLanguage").val() + "' data-key='" + row.Key + "' data-href='/SysAdmin/Languages/EditText' class='btn btn-default btn-xs js-trigger-modal' title='Editar texto' ><i class='fa fa-edit'></i></a>";
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
        var currentRowSelected;
        $('#languageTextsTable tbody').on('click', '.js-trigger-modal', function () {
            var row = $(this).parent().parent();
            currentRowSelected = {
                data : table.row (row).data (),
                row : row
            };
            var href = $(this).data("href");
            var data = {
                LanguageName : $ (this).data ("lang"),
                Key : $ (this).data ("key"),
                Source : $ (this).data ("source"),
                Value : $ (this).data ("current")
            };
            modalInstance.open(href, data);
        });

        function modalHandler(event) {
            switch (event.detail.info.modalType) {
                case "MODAL_CHANGE_TEXT":
                    currentRowSelected.data.TargetValue = event.detail.info.Value;
                    table.row(currentRowSelected.row).data(currentRowSelected.data).draw(false);
                    console.log(currentRowSelected);
                    break;
                default:
                    break;
            }
        }

        var languageTextPage = {
            modalHadler:modalHandler
        }

        document.addEventListener('modalClose', languageTextPage.modalHandler);

        $('body').on('change', '.js-select', function () {

            var src = $("#Source").val();
            var target = $("#SelectedTargetLanguage").val();
            var sourceAbp = $("#SelectedSourceLanguage").val();

            table.ajax.url("/SysAdmin/Languages/" + "GetLanguageTextsForTable?Source=" + src + "&TargetLang=" + target + "&SourceLang=" + sourceAbp).load();
        });


    });


})();