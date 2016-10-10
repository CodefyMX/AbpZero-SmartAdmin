
(function () {

    $(document)
        .ready(function () {
            $("#setCategories").on("submit", function (e) {
                e.preventDefault();
                var data = {
                    MenuId: $("#MenuId").val(),
                    AvailableCategories: []
                };
                $(".js-set-category").each(function (i, element) {
                    var $element = $(element);
                    data.AvailableCategories.push({
                        Checked: $element.is(":checked"),
                        CategoryId: $element.data("category")
                    });
                });

                abp.services.cms.menuService.setMenuSectionsFromCategories(data)
                    .done(function () {
                        window.location.reload();

                    });
            });
        });


})();