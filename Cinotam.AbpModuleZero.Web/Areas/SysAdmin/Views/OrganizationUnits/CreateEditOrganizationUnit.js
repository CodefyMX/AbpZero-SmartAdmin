(function () {
    var modalType = "MODAL_CREATE_EDIT_ORG_UNIT";
    $(document)
        .ready(function() {

            var _organizationUnitsAppService = abp.services.app.organizationUnits;
            var $form = $("#createEditOrgUnit");

            $form.on("submit",
                function (e) {
                    var $self = $(this);
                    e.preventDefault();
                    var data = $self.serializeFormToObject();

                    _organizationUnitsAppService.createOrEditOrgUnit(data)
                        .done(function () {
                            window.modalInstance.close({}, modalType);
                        });

                });
        });
})();