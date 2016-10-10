(function () {

    var columns = [
        {
            "data": "Title"
        },
        {
            "data": "CategoryName"
        }, {
            "data": "TemplateName"
        }
    ];

    var columnDefinitions = [
        {
            "render": function (data, type, row) {
                var lang = "";
                if (row.Langs.length <= 0) {
                    return LSys("NoLangAvaiableYetForThisPage");
                }
                row.Langs.forEach(function (dLanf) {
                    lang = lang + "<i title='" + dLanf.LangCode + "' class='" + dLanf.LangIcon + "'></i> ";
                });
                return lang;
            },
            "targets": 3
        },
        {
            className: "text-center",
            "render": function (data, type, row) {
                return "<a href='/SysAdmin/Cms/PageConfig/" + row.Id + "' class='btn btn-primary btn-xs'><i class='fa fa-edit'></i></a>";
            },
            "targets": 4
        },
        {
            "orderable": false,
            "targets": [1, 2]
        }
    ];

    var dataTablesConfig = new DatatablesConfig({
        Url: "/SysAdmin/Cms/" + "GetPagesTable",
        DisplayLength: 10,
        OnInitComplete: function () { },
        Columns: columns,
        ColumnDefinitions: columnDefinitions,
        Element: $("#pagesTable")
    });

    var myPagesPageConfiguration = {
        DataTablesConfig: dataTablesConfig

    }

    var table = $("#pagesTable")
.DataTable(myPagesPageConfiguration.DataTablesConfig);

})();