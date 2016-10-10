
(function () {

    window.modalInstance = new abp.app.bootstrap.modal(null, window.modalOptions);

    $("#changeProfilePicture")
        .on("submit",
            function (e) {
                e.preventDefault();
                var data = new FormData(this);
                var id = $("#Id").val();
                abp.ui.setBusy($("#imageWrapped"), upload(data, id));
            });
    $("#createEditForm").on("submit", function (e) {

        e.preventDefault();
        var url = $(this).attr("action");
        var data = $(this).serializeFormToObject();
        abp.ui.setBusy($("#createEditForm"), abp.ajax({
            url: url,
            data: JSON.stringify(data)
        }).done(function () {
            abp.message.success(LSys("ProfileModified"), LSys("Success"));
        }));
    });
    function upload(data, id) {
        return abp.ajax({
            type: "POST",
            url: "/SysAdmin/Users/" + "ChangeProfilePicture/" + id,
            data: data,
            contentType: false,
            cache: false,
            processData: false
        })
            .done(function (response) {
                abp.message.success(LSys("ProfilePicModified"), LSys("Success"));
                $('#profilePicture').attr("src", response);
            });
    }

    document.addEventListener('modalClose', modalHandler);
    function modalHandler(event) {
        switch (event.detail.info.modalType) {
            case "USER_PASSWORD_CHANGED":
                abp.message.success(LSys("PasswordChanged"), LSys("Success"));
                break;
            default:
                console.log("Event unhandled");
        }
    }

})();
(function () {
    $(document)
        .ready(function () {
            var element = $("#myNotifications");
            element.text("");
            abp.services.app.user.getMyNotifications(2, null).done(function (response) {
                response.notifications.forEach(function (userNotification) {
                    notificationService.printNotificationInList(userNotification, element);
                });
                notificationService.startListening();
            });
        });
})();