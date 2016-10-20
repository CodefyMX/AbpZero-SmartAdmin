(function () {
    var modalType = "USER_PHONE_CHANGED";
    $(document)
        .ready(function () {

            var _userAppService = abp.services.app.user;
            var $form = $("#confirmPhoneForm");
            $form.on("submit",
                function (e) {
                    var $self = $(this);
                    e.preventDefault();

                    var data = $self.serializeFormToObject();

                    abp.ui.setBusy($form, _userAppService.confirmPhone(data).done(function (response) {
                        window.modalInstance.close({}, modalType);
                    }));
                });
        });
})();