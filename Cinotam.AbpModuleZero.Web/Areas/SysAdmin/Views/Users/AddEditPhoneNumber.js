(function () {
    var modalType = "PHONE_CHANGE_REQUEST";
    var modalTypeSameNumberResult = "SAME_PHONE_NUMBER";
    $(document)
        .ready(function () {
            var countryCode;
            $("#CountryPhoneCode")
                .on("change",
                    function (e) {
                        var option = $(this).find(':selected');
                        countryCode = $(option).data("countrycode");
                        
                    });
            var _userAppService = abp.services.app.user;
            var $form = $("#changePhoneForm");
            $form.on("submit",
                function (e) {
                    var $self = $(this);
                    e.preventDefault();
                    if (!countryCode) {
                        var option = $("#CountryPhoneCode").find(":selected");
                        countryCode = $(option).data("countrycode");
                    }
                    var data = $self.serializeFormToObject();
                    data.CountryCode = countryCode;
                    console.log(data);
                    abp.ui.setBusy($form, _userAppService.addPhoneNumber(data).done(function (response) {

                        //resultType = 0 .- SamePhoneNumber
                        //resultType = 1 .- ChangePhoneNumber
                        //resultType = 2 .- NewPhoneNumber
                        if (response.resultType === 0) {
                            window.modalInstance.close({}, modalTypeSameNumberResult);
                        } else {

                            window.modalInstance.sendCloseEvent(response , modalType);
                        }

                    }));
                });
        });
})();