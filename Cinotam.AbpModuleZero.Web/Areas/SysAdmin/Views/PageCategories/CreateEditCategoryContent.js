
(function () {
    var _categoriesAppService = abp.services.cms.categoryService;
    $(document)
        .ready(function() {
            var $form = $("#createCategoryForm");
            var $langInputs = $(".js-lang-input");
            $form.on("submit", function (e) {
                e.preventDefault();


                var availableLangs = [];

                $langInputs.each(function (i, element) {
                    var $element = $(element);
                    var langInput = {
                        Lang: $element.data("lang"),
                        MenuId: $element.data("id"),
                        Text: $element.val()
                    }
                    availableLangs.push(langInput);
                });
                var data = {
                    Name: $("#Name").val(),
                    DisplayName: $("#DisplayName").val(),
                    LanguageInputs: availableLangs
                }
                abp.ui.setBusy($form, _categoriesAppService.addEditCategory(data).done(function () {
                    window.location.reload();
                }));

            });

        });

})();