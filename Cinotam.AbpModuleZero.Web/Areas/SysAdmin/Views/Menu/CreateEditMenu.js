
(function () {

    $(document)
        .ready(function () {
            var $form = $("#createMenuForm");
            $form.on("submit", function (e) {
                var _menuAppService = abp.services.cms.menuService;
                
                var $langInputs = $(".js-lang-input");
                e.preventDefault();
                var availableLangs = [];


                $langInputs.each(function (i, element) {
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
                abp.ui.setBusy($form, _menuAppService.addMenu(data).done(function () {
                    window.location.reload();
                }));

            });
        });

    
})();