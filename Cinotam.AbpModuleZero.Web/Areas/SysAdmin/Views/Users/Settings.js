
(function () {
    $(document)
        .ready(function () {

            var _settingsAppService = abp.services.app.settings;
            var $themeSelect = $("#themeSelector");
            var $body = $("body");
            var $themeSelectOption = $("#themeSelector option:selected");
            $themeSelect.change(function () {
                $body.attr("class", "");
                var value = $themeSelectOption.val();
                $body.attr("class", value);
                abp.ui.setBusy($body, _settingsAppService.changeTheme(value));
            });
            $body.on("click", "[data-subscription]", function () {
                var $elm = $(this);
                var isSubscribed = $elm.data("is-subscribed");
                var notificationName = $elm.data("subscription-name");
                if (isSubscribed) {
                    unSubscribe(notificationName, $elm);
                } else {
                    subscribe(notificationName, $elm);
                }
            });
            function subscribe(notificationName, $elm) {
                abp.ui.setBusy($body, _settingsAppService.subscribeToNotification(notificationName).done(function () {
                    $elm.text(LSys("UnSubscribe"));
                    $elm.data("is-subscribed", true);
                }));
            }
            function unSubscribe(notificationName, $elm) {
                abp.ui.setBusy($body, _settingsAppService.unSubscribeToNotification(notificationName).done(function () {
                    $elm.text(LSys("Subscribe"));
                    $elm.data("is-subscribed", false);
                }));
            }

            var subscriptionElements = $("[data-subscription]");

            subscriptionElements.each(function () {
                var $elm = $(this);
                var val = $elm.data("subscription-name");
                abp.services.app.settings.isSubscribed(val).done(function (response) {
                    if (response) {
                        $elm.text(LSys("UnSubscribe"));
                        $elm.data("is-subscribed", true);
                        $elm.prop("checked", true);
                    } else {
                        $elm.text(LSys("Subscribe"));
                        $elm.data("is-subscribed", false);
                        $elm.prop("checked", false);
                    }
                });
            });

        });
})();