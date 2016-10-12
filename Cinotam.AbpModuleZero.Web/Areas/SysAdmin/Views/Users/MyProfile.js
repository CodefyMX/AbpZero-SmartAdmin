
(function () {
    $(document)
        .ready(function () {
            var $changeProfilePictureForm =$("#changeProfilePicture");
            var $createEditForm = $("#createEditForm");
            var $profilePictureElement = $('#profilePicture');
            var $notificationsElement = $("#myNotifications");
            var $wrapper = $("#imageWrapped");
            var _userAppService = abp.services.app.user;
            $changeProfilePictureForm.on("submit",
                    function (e) {
                        var self = this;
                        e.preventDefault();
                        var data = new FormData(self);
                        var id = $("#Id").val();
                        abp.ui.setBusy($wrapper, upload(data, id));
                    });
            $createEditForm.on("submit", function (e) {

                e.preventDefault();

                var $self = $(this);
                var url = $self.attr("action");
                var data = $self.serializeFormToObject();
                abp.ui.setBusy($createEditForm, abp.ajax({
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
                        $profilePictureElement.attr("src", response);
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
            _userAppService.getMyNotifications(2, null).done(function (response) {
                $notificationsElement.text("");
                response.notifications.forEach(function (userNotification) {
                    notificationService.printNotificationInList(userNotification, $notificationsElement);
                });
                notificationService.startListening();
            });
        });
})();