
(function () {
    $("#createMenuForm").on("submit", function (e) {
        e.preventDefault();
        var availableLangs = [];
        $(".js-lang-input")
            .each(function (i, element) {
                var $element = $(element);
                var langInput = {
                    Lang: $element.data("lang"),
                    MenuId: $element.data("id"),
                    DisplayText: $element.val(),
                    Id: $element.data("content-id")
                }
                availableLangs.push(langInput);
            });
        var data = {
            Name: $("#Name").val(),
            AvailableLangs: availableLangs,
            IsActive: $("#IsActive").is(":checked"),
            Id: $("#Id").val()
        }
        abp.ui.setBusy($("#createMenuForm"), abp.services.cms.menuService.addMenu(data).done(function () {
            window.location.reload();
        }));

    });
})();