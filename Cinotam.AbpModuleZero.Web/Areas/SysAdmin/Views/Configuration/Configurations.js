(function () {
    $(document).ready(function () {

        var $settingsForm = $("#settings");
        var $settingsInputSelector = $("#settings input");
        var _settingsService = abp.services.app.settings;
        $settingsForm.on("submit", function (e) {
            var data = [];
            e.preventDefault();
            $settingsInputSelector.each(function () {
                var $element = $(this);
                var key = $element.data("key");
                var scope = $element.data("scope");
                var value = $element.val();
                data.push({
                    Key: key,
                    Value: value,
                    SettingScopes: scope
                });

            });
            _settingsService.createEditSetting(data).done(function () {
                window.location.reload();
            });
        });
    });
})();