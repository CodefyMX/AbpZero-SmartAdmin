(function () {
    var modalType = "MODAL_CHANGE_TEXT";
    $(document).ready(function () {
        $("#editText").on("submit", function (e) {
            e.preventDefault();
            var data = {
                Value: $("#Value").val(),
                Key: $("#Key").val(),
                LanguageName: $("#LanguageName").val(),
                Source: $("#Source").val()

            };
            console.log(data);
            abp.ui.setBusy(this, abp.services.app.language.addEditLocalizationText(data).done(function () {
                modalInstance.close(data, modalType);
            }));
        });
    });
})();