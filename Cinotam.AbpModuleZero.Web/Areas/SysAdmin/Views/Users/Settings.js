
(function () {
    $(document)
        .ready(function () {

            $("#themeSelector").change(function () {
                $("body").attr("class", "");
                var value = $("#themeSelector option:selected").val();
                $("body").attr("class", value);
                abp.ui.setBusy("body", abp.services.app.settings.changeTheme(value));
            });
            $("body").on("click", "[data-subscription]", function () {
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
                abp.ui.setBusy("body", abp.services.app.settings.subscribeToNotification(notificationName).done(function () {
                    $elm.text(LSys("UnSubscribe"));
                    $elm.data("is-subscribed", true);
                }));
            }
            function unSubscribe(notificationName, $elm) {
                abp.ui.setBusy("body", abp.services.app.settings.unSubscribeToNotification(notificationName).done(function () {
                    $elm.text(LSys("Subscribe"));
                    $elm.data("is-subscribed", false);
                }));
            }
            $("[data-subscription]").each(function () {
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