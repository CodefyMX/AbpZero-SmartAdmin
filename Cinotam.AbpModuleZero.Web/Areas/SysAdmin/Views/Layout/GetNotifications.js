
(function () {

    $(document)
        .ready(function () {
            notificationService.initView();
            abp.event.on('abp.notifications.received', function (userNotification) {
                notificationService.printNotificationInList(userNotification);
            });
        });
})();
