(function () {
    var modalType = "MODAL_CREATE_EDIT_ORG_UNIT";
    $("#createEditOrgUnit")
        .on("submit",
            function (e) {
                e.preventDefault();
                var data = $(this).serializeFormToObject();

                abp.services.app.organizationUnits.createOrEditOrgUnit(data)
                    .done(function () {
                        window.modalInstance.close({}, modalType);
                    });

            });
})();