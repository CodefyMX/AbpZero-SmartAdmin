(function () {
    var modalType = "USER_PASSWORD_CHANGED";
    $(document)
        .ready(function () {
            $("#changePasswordForm")
                .on("submit",
                    function (e) {

                        e.preventDefault();
                        if ($(".js-confirm-password").val() !== $(".js-password").val()) {
                            abp.message.error(LSys("PasswordsNotMatch"), LSys("Error"));
                        } else {
                            var form = this;

                            var data = $(form).serializeFormToObject();

                            abp.ui.setBusy(form, abp.services.app.user.changePassword(data).done(function () {
                                window.modalInstance.close({}, modalType);
                            }));
                        }
                    });
        });

})();