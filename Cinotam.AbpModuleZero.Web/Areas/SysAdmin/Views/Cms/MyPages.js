(function () {


    var table = $("#pagesTable")
.DataTable({
    "bServerSide": true,
    "bPaginate": true,
    "sPaginationType": "full_numbers", // And its type.
    "iDisplayLength": 10,
    "ajax": "/SysAdmin/Cms/" + "GetPagesTable",
    "autoWidth": true,
    "preDrawCallback": function () {
        // Initialize the responsive datatables helper once.
        if (!responsiveHelper_dt_roles) {
            responsiveHelper_dt_roles = new
                ResponsiveDatatablesHelper($('#pagesTable'), breakpointDefinition);
        }
    },
    "rowCallback": function (nRow) {
        responsiveHelper_dt_roles.createExpandIcon(nRow);
    },
    columnDefs: [
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
    ],
    "drawCallback": function (oSettings) {
        responsiveHelper_dt_roles.respond();
    },
    language: window.dataTablesLang,
    columns: [
        {
            "data": "Title"
        },
        {
            "data": "CategoryName"
        }, {
            "data": "TemplateName"
        }
    ]
});

})();