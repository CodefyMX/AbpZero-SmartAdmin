(function () {
    var modalType = "MODAL_CHANGE_TEXT";
    var $form = $("#editText");
    $(document).ready(function () {
        $form.on("submit", function (e) {
            e.preventDefault();
            var data = {
                Value: $("#Value").val(),
                Key: $("#Key").val(),
                LanguageName: $("#LanguageName").val(),
                Source: $("#Source").val()

            };
            abp.ui.setBusy($form, abp.services.app.language.addEditLocalizationText(data).done(function () {
                modalInstance.close(data, modalType);
            }));
        });
    });
})();