(function () {
    var _tenantAppService = abp.services.app.tenant;
    var editionSet = "EDITION_SET";
    $(document)
        .ready(function () {

            var $form = $("#setTenantEdition");

            $form.on("submit",
                function (e) {

                    e.preventDefault();


                    var editionId = $('input[name=edition]:checked', "#setTenantEdition").val();
                    var tenantId = $("#TenantId").val();
                    var data = {
                        tenantId: tenantId,
                        editionId: editionId
                    };


                    abp.ui.setBusy($form, _tenantAppService.setTenantEdition(data).done(function () {

                        window.modalInstance.close({}, editionSet);

                    }));

                });


        });


})();