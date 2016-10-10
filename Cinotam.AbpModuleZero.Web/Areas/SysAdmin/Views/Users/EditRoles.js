
(function () {
    var modalType = "MODAL_ROLES_SET";
    $(document).ready(function () {
        $("#selectRoles").on("submit", function (e) {
            e.preventDefault();
            var data = {
                userId: $("#UserId").val(),
                roles: []
            };

            $("#selectRoles input:checked").each(function () {
                data.roles.push($(this).val());
            });

            abp.services.app.user.setUserRoles(data).done(function () {
                window.modalInstance.close({}, modalType);
            });

        });
    });
})();