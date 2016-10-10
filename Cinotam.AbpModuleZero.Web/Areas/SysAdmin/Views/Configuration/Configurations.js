(function () {
    $(document).ready(function () {



        $("#settings").on("submit", function (e) {
            var data = [];
            e.preventDefault();
            $("#settings input").each(function () {
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
            abp.services.app.settings.createEditSetting(data).done(function () {
                window.location.reload();
            });
        });
    });
})();