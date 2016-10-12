(function () {
    "use strict";

    $(document).ready(function () {
        var $table = $("#languageTextsTable");
        var $body = $("body");
        drawBreadCrumb([LSys("Language"), LSys("Texts")]);

        var source = $("#Source").val();
        var targetLang = $("#SelectedTargetLanguage").val();
        var sourceLang = $("#SelectedSourceLanguage").val();
        var columns = [
            { "data": "Id" },
            {
                "data": "Key"
            },
            { "data": "SourceValue" },
            { "data": "TargetValue" }
        ];
        var columnDefs = [
            {
                className: "text-center",
                "render": function(data, type, row) {
                    return " <a data-current='" +
                        row.TargetValue +
                        "' data-source='" +
                        $("#Source").val() +
                        "' data-lang='" +
                        $("#SelectedTargetLanguage").val() +
                        "' data-key='" +
                        row.Key +
                        "' data-href='/SysAdmin/Languages/EditText' class='btn btn-default btn-xs js-trigger-modal' title='Editar texto' ><i class='fa fa-edit'></i></a>";
                },
                "targets": 0
            }
        ];
        var dataTablesConfig = new DatatablesConfig({
            Url: "/SysAdmin/Languages/" + "GetLanguageTextsForTable?Source=" + source + "&TargetLang=" + targetLang + "&SourceLang=" + sourceLang,
            Columns: columns,
            ColumnDefinitions: columnDefs,
            Element: $('#languageTextsTable'),
            OnInitComplete: function () { },
            DisplayLength: 10,
            Ajax: false //Ajax calls to the server disabled
        });
        var languageTextPage = {
            modalHandler: modalHandler,
            dataTablesConfig: dataTablesConfig
        }


        var table = $table.DataTable(languageTextPage.dataTablesConfig);

        var currentRowSelected;
        $body.on('click', '.js-trigger-modal', function () {
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
                    break;
                default:
                    break;
            }
        }

        

        document.addEventListener('modalClose', languageTextPage.modalHandler);

        $body.on('change', '.js-select', function () {

            var src = $("#Source").val();
            var target = $("#SelectedTargetLanguage").val();
            var sourceAbp = $("#SelectedSourceLanguage").val();

            table.ajax.url("/SysAdmin/Languages/" + "GetLanguageTextsForTable?Source=" + src + "&TargetLang=" + target + "&SourceLang=" + sourceAbp).load();
        });


    });


})();