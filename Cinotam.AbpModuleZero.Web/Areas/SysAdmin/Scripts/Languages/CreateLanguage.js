(function () {
    var modalType = "LANGUAGE_CREATED";
    function format(icon) {
        var originalOption = icon.element;
        return '<i class="' + $(originalOption).data('icon') + '"></i> ' + icon.text;
    }
    $(document).ready(function () {

        $("#createEditLang").on("submit", function (e) {
            e.preventDefault();
            var icon = $("#icon").val();
            var name = $("#name").val();
            var displayNameText = $("#name option:selected").text();
            var displayName = displayNameText.substring(0, (displayNameText.indexOf("(") - 1));
            var data = {
                Icon: icon,
                LangCode: name,
                DisplayName: displayName
            }
            abp.services.app.language.addLanguage(data)
                .done(function () {
                    modalInstance.close({}, modalType);
                });
        });


        $('.select2').select2({
            formatResult: format
        });
    });

})();