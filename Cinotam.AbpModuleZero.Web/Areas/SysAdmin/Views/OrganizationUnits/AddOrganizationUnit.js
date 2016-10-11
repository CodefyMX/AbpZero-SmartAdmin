(function () {
    var modalType = "MODAL_CREATE_EDIT_ORG_UNIT";
    $(document)
        .ready(function() {
            var $form = $("#createEditOrgUnit");
            $form.on("submit",
                function (e) {
                    e.preventDefault();
                    var data = $(this).serializeFormToObject();

                    abp.ui.setBusy($form, abp.services.app.organizationUnits.createOrEditOrgUnit(data)
                        .done(function () {
                            window.modalInstance.close({}, modalType);
                        }));

                });
        });
    
})();