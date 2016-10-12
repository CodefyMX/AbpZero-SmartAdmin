
(function () {
    var modalType = "MODAL_ROLES_SET";
    $(document).ready(function () {

        var _userAppService = abp.services.app.user;
        var $form = $("#selectRoles");

        $form.on("submit", function (e) {
            e.preventDefault();
            var data = {
                userId: $("#UserId").val(),
                roles: []
            };

            var inputsChecked = $("#selectRoles input:checked");

            inputsChecked.each(function () {
                var $self = $(this);

                var checkedElementValue = $self.val();

                data.roles.push(checkedElementValue);
            });

            _userAppService.setUserRoles(data).done(function () {
                window.modalInstance.close({}, modalType);
            });

        });
    });
})();