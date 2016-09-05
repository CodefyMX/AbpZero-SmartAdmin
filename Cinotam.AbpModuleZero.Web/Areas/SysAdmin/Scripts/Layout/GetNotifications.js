
(function () {

    $(document)
        .ready(function () {
            notificationService.initView();
            notificationService.startListening();
            abp.event.on('abp.notifications.received', function (userNotification) {
                notificationService.printNotificationInList(userNotification);
            });
        });
})();
