(function () {

    var modalType = "LANGUAGE_CREATED";
    function format(icon) {
        var originalOption = icon.element;
        return '<i class="' + $(originalOption).data('icon') + '"></i> ' + icon.text;
    }
    $(document).ready(function () {
        var $form = $("#createEditLang");
        var _languageAppService = abp.services.app.language;
        $form.on("submit", function (e) {
            e.preventDefault();
            var icon = $("#icon").val();
            var name = $("#name").val();
            var optionSelected = $("#name option:selected");
            var displayNameText = optionSelected.text();
            var displayName = displayNameText.substring(0, (displayNameText.indexOf("(") - 1));
            var data = {
                Icon: icon,
                LangCode: name,
                DisplayName: displayName
            }
            abp.ui.setBusy($form, _languageAppService.addLanguage(data)
                 .done(function () {
                     modalInstance.close({}, modalType);
                 }));

        });

        intializeSelect2Elements();

    });
    function intializeSelect2Elements() {
        $('.select2').select2({
            formatResult: format
        });
    }
})();