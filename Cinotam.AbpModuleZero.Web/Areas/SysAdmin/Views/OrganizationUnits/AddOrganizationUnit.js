(function () {
    var modalType = "MODAL_CREATE_EDIT_ORG_UNIT";
    $(document)
        .ready(function () {

            var _organizationUnitsAppService = abp.services.app.organizationUnits;
            var $form = $("#createEditOrgUnit");
            $form.on("submit",
                function (e) {
                    e.preventDefault();
                    var data = $(this).serializeFormToObject();

                    abp.ui.setBusy($form, _organizationUnitsAppService.createOrEditOrgUnit(data)
                        .done(function () {
                            window.modalInstance.close({}, modalType);
                        }));

                });
        });
    
})();