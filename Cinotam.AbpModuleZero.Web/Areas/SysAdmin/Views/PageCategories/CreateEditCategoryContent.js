
(function () {

    $("#createCategoryForm").on("submit", function (e) {
        e.preventDefault();
        var availableLangs = [];
        $(".js-lang-input")
            .each(function (i, element) {
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
        abp.ui.setBusy($("#createCategoryForm"), abp.services.cms.categoryService.addEditCategory(data).done(function () {
            window.location.reload();
        }));

    });


})();