
(function () {
    $(document).ready(function () {

        var table = $("#categoriesTable")
        .DataTable({
            "bServerSide": true,
            "bPaginate": true,
            "sPaginationType": "full_numbers", // And its type.
            "iDisplayLength": 10,
            "ajax": "/SysAdmin/PageCategories/" + "GetCategories",
            "autoWidth": true,
            "preDrawCallback": function () {
                // Initialize the responsive datatables helper once.
                if (!responsiveHelper_dt_roles) {
                    responsiveHelper_dt_roles = new
                        ResponsiveDatatablesHelper($('#categoriesTable'), breakpointDefinition);
                }
            },
            "rowCallback": function (nRow) {
                responsiveHelper_dt_roles.createExpandIcon(nRow);
            },
            columnDefs: [
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
                        return "<a data-modal href='/SysAdmin/PageCategories/CreateEditCategoryContent/" + row.Id + "' class='btn btn-primary btn-xs'><i class='fa fa-edit'></i></a> <a data-id=" + row.Id + " class='btn btn-danger btn-xs js-delete-category'><i class='fa fa-times'></i></a>";
                    },
                    "targets": 3
                }

            ],
            "drawCallback": function (oSettings) {
                responsiveHelper_dt_roles.respond();
            },
            language: window.dataTablesLang,
            columns: [
                {
                    "data": "Name"
                },
                {
                    "data": "DisplayName"
                }
            ]
        });


        $("body").on("click", ".js-delete-category", function () {
            var id = $(this).data("id");
            abp.message.confirm(LSys("Delete"), LSys("AreYouSure"), function (response) {
                if (response) {
                    abp.ui.setBusy($("#categoriesTable"), abp.services.cms.categoryService.removeCategory(id).done(function () {
                        abp.notify.warn(LSys("CategoryRemoved"));
                    }));
                }
            });
        });

    });

})();