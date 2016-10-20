(function () {
    var modalType = "";
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
                        window.modalInstance.close({}, modalType);
                        window.modalInstance.open("/SysAdmin/Users/ConfirmPhone/?phoneNumber=" + response.phoneNumber + "&userId=" + response.userId);
                    }));
                });
        });
})();