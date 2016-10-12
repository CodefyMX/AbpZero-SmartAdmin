
(function () {
    var _menuAppService = abp.services.cms.menuService;
    $(document)
        .ready(function () {

            var $form = $("#setCategories");
            var $setCategoryElements = $(".js-set-category");
            $form.on("submit", function (e) {
                e.preventDefault();
                var data = {
                    MenuId: $("#MenuId").val(),
                    AvailableCategories: []
                };
                $setCategoryElements.each(function (i, element) {
                    var $element = $(element);
                    data.AvailableCategories.push({
                        Checked: $element.is(":checked"),
                        CategoryId: $element.data("category")
                    });
                });

                _menuAppService.setMenuSectionsFromCategories(data)
                    .done(function () {
                        window.location.reload();

                    });
            });
        });


})();