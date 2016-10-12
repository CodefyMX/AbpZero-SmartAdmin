
(function () {
    $(document).ready(function () {

        var _categoriesAppService = abp.services.cms.categoryService;
        var $table = $("#categoriesTable");
        var $body = $("body");
        var columns = [
            {
                "data": "Name"
            },
            {
                "data": "DisplayName"
            }
        ];

        var columnDefinitions = [
            {
                "render": function (data, type, row) {
                    var lang = "";
                    if (row.Languages.length <= 0) {
                        return LSys("NoLangAvaiableYetForThisCategory");
                    }
                    row.Languages.forEach(function (dLanf) {
                        lang = lang + "<i title='" + dLanf.LangCode + "' class='" + dLanf.LangIcon + "'></i> ";
                    });
                    return lang;
                },
                "targets": 2


            }, {
                className: "text-center",
                "render": function (data, type, row) {
                    return "<a data-modal href='/SysAdmin/PageCategories/CreateEditCategoryContent/" +
                        row.Id +
                        "' class='btn btn-primary btn-xs'><i class='fa fa-edit'></i></a> <a data-id=" +
                        row.Id +
                        " class='btn btn-danger btn-xs js-delete-category'><i class='fa fa-times'></i></a>";
                },
                "targets": 3
            }
        ];

        var dataTablesConfig = new DatatablesConfig({
            Url: "/SysAdmin/PageCategories/" + "GetCategories",
            Columns: columns,
            ColumnDefinitions: columnDefinitions,
            Element: $table,
            OnInitComplete: function () { },
            DisplayLength: 10
        });

        var myCategoriesPage = {
            dataTablesConfig: dataTablesConfig
        }


        $table
        .DataTable(myCategoriesPage.dataTablesConfig);

        $body.on("click", ".js-delete-category", function () {
            var id = $(this).data("id");
            abp.message.confirm(LSys("Delete"), LSys("ConfirmQuestion"), function (response) {
                if (response) {
                    abp.ui.setBusy($form, _categoriesAppService.removeCategory(id).done(function () {
                        abp.notify.warn(LSys("CategoryRemoved"));
                    }));
                }
            });
        });

    });

})();