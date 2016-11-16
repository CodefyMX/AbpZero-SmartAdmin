(function () {
    $(document).ready(function () {
        var loader = new divAjaxLoader();

        var clearCache = $(".js-clear-cache");

        var $settingsForm = $("#settings");
        var _settingsService = abp.services.app.settings;

        clearCache.click(function() {
            abp.ui.setBusy($settingsForm, _settingsService.clearCaches().done(function () {
                abp.notify.success(LSys("CacheCleaned"), LSys("Success"));
            }));
        });

        $settingsForm.on("submit", function (e) {

            var $settingsInputSelector = $("#settings input");
            var data = [];
            e.preventDefault();
            $settingsInputSelector.each(function () {
                var $element = $(this);
                var key = $element.data("key");
                var scope = $element.data("scope");
                var isCheckBox = $element.data("is-checkbox");

                var value = $element.val();
                if (isCheckBox) {
                    value = $element.is(":checked");
                }
                data.push({
                    Key: key,
                    Value: value,
                    SettingScopes: scope
                });

            });
            abp.ui.setBusy($settingsForm, _settingsService.createEditSetting(data).done(function () {
                abp.notify.success(LSys("ChangesSaved"), LSys("Success"));
            }));
        });
    });
})();