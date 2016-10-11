(function () {
    var modalType = "MODAL_USER_CREATED";
    $("#createEditForm").on("submit", function (e) {

        e.preventDefault();
        var url = $(this).attr("action");
        var data = $(this).serializeFormToObject();
        data.IsActive = $("#IsActive").is(":checked");
        data.SendNotificationMail = $("#SendNotificationMail").is(":checked");
        abp.ui.setBusy($("#createEditForm"), abp.ajax({
            url: url,
            data: JSON.stringify(data)
        }).done(function () {
            window.modalInstance.close({}, modalType);
        }));
    });
})();