(function () {
    var modalType = "PHONE_CHANGE_REQUEST";
    $(document)
        .ready(function () {

            var _userAppService = abp.services.app.user;
            var $form = $("#changePhoneForm");
            $form.on("submit",
                function (e) {
                    var $self = $(this);
                    e.preventDefault();

                    var data = $self.serializeFormToObject();

                    abp.ui.setBusy($form, _userAppService.addPhoneNumber(data).done(function (response) {
                        window.modalInstance.sendCloseEvent(
                            {
                                PhoneNumber: response.phoneNumber,
                                UserId: response.userId
                        }, modalType);
                    }));
                });
        });
})();