(function () {

    var modalType = "MODAL_CHANGE_TEXT";
    $(document).ready(function () {
        var $form = $("#editText");
        var _languageService = abp.services.app.language;
        $form.on("submit", function (e) {
            e.preventDefault();
            var data = {
                Value: $("#Value").val(),
                Key: $("#Key").val(),
                LanguageName: $("#LanguageName").val(),
                Source: $("#Source").val()

            };
            abp.ui.setBusy($form, _languageService.addEditLocalizationText(data).done(function () {
                modalInstance.close(data, modalType);
            }));
        });
    });
})();