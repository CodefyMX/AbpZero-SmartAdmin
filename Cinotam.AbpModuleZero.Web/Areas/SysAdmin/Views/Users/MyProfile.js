
(function () {
    $(document)
        .ready(function () {
            var $changeProfilePictureForm = $("#changeProfilePicture");
            var $createEditForm = $("#createEditForm");
            var $profilePictureElement = $('#profilePicture');
            var $notificationsElement = $("#myNotifications");
            var $wrapper = $("#imageWrapped");
            var $btnTwoFactorToggle = $(".js-toggle-twofactor");
            var $profilePicture = $("#profilePicture");
            var $profilePictureFile = $("#profilePictureFile");


            var _userAppService = abp.services.app.user;
            $changeProfilePictureForm.on("submit",
                    function (e) {
                        var self = this;
                        e.preventDefault();
                        var data = new FormData(self);
                        var id = $("#Id").val();
                        abp.ui.setBusy($wrapper, upload(data, id));
                    });

            window.uploadProfilePicture = function (element) {

                element.parentNode.nextSibling.value = element.value;
                $changeProfilePictureForm.submit();

            }

            $profilePicture.click(function () {
                $profilePictureFile.click();
            });

            $btnTwoFactorToggle.click(function () {
                var id = $(this).data("id");

                abp.ui.setBusy($createEditForm, _userAppService.enableOrDisableTwoFactorAuthForUser(id).done(function () {
                    window.location.reload();
                }));
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
                    case "PHONE_CHANGE_REQUEST":
                        console.log(event);
                        window.modalInstance.openInBody("/SysAdmin/Users/ConfirmPhone/?phoneNumber=" + event.detail.info.phoneNumber + "&userId=" + event.detail.info.userId + "&countryCode=" + event.detail.info.countryCode + "&countryPhoneCode=" + event.detail.info.countryPhoneCode);
                        break;
                    case "PHONE_CONFIRMED":
                        abp.notify.success(LSys("PhoneConfirmed"), LSys("Success"));
                        window.modalInstance.close({});
                        break;
                    case "SAME_PHONE_NUMBER":
                        abp.notify.success(LSys("PhoneAlreadyRegistered"), LSys("Success"));
                        break;
                    default:
                        console.log("Event unhandled");
                }
            }



            _userAppService.getMyNotifications(2, null).done(function (response) {

                $notificationsElement.text("");
                response.notifications.forEach(function (userNotification) {
                    notificationService.printNotificationInListSWF(userNotification, $notificationsElement);
                });

            });
            $("#searchNotifications")
                .on("keyup",
                    function (e) {
                        var value = $(this).val();
                        console.log(value);

                        _userAppService.getMyNotifications(10, null, value).done(function (response) {
                            $notificationsElement.text("");
                            response.notifications.forEach(function (userNotification) {

                                var format = "{0}";

                                notificationService.printNotificationInListSWF(userNotification, $notificationsElement, format);
                            });
                        });

                    });
        });
})();