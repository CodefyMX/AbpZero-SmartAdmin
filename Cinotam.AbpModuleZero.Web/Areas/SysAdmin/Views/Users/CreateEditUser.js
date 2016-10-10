(function () {
    var modalType = "MODAL_USER_CREATED";
    var modalTypeDeleted = "MODAL_USER_DELETED";
    var fullName = $("#Name").val() + " " + $("#Surname").val();
    $("body").on("click", ".js-delete-user", function () {
        var id = $(this).data("id");
        var confirmDelete = abp.utils.formatString(LSys("ConfirmDeleteUser"), fullName);
        abp.message.confirm(confirmDelete, LSys("ConfirmQuestion"), function (response) {
            if (response) {
                abp.ui.setBusy($("#createEditForm"), abp.services.app.user.deleteUser(id).done(function () {
                    window.modalInstance.close({}, modalTypeDeleted);
                }));
            }
        });
    });

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