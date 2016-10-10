
(function () {
    var modalType = "MODAL_ROLE_CREATED";
    var modalTypeDelete = "MODAL_ROLE_DELETED";
    $(document).ready(function () {

        var roleName = $("#DisplayName").val();
        $("body").on("click", ".js-delete-role", function () {
            var id = $(this).data("id");
            var message = abp.utils.formatString(LSys("RoleDeleteMessage"), roleName);

            abp.message.confirm(message, LSys("ConfirmQuestion"), function (response) {
                if (response) {
                    abp.ui.setBusy($("#createEditRole"), abp.services.app.role.deleteRole(id).done(function () {
                        window.modalInstance.close({}, modalTypeDelete);
                    }));
                }
            });
        });


        $("#container")
            .jstree({
                "checkbox": {
                    "keep_selected_style": false
                },
                'plugins': ["wholerow", "html_data", "checkbox", "ui"],
                'core': {
                    'themes': {
                        'name': 'proton',
                        'responsive': true
                    }
                }
            });
        $('#container').on('ready.jstree', function () {
            $("#container").jstree("open_all");
        });
        $("#createEditRole").on("submit", function (e) {
            var data = {
                AssignedPermissions: [],
                DisplayName: $("#DisplayName").val(),
                Id: $("#Id").val()
            }
            e.preventDefault();
            var selected = $("#container").jstree('get_selected');
            $(selected).each(function (index, v) {
                console.log(index);
                data.AssignedPermissions.push({
                    Name: v,
                    Granted: true
                });
            });
            abp.ui.setBusy($("#createEditForm"), abp.services.app.role.createEditRole(data).done(function () {
                window.modalInstance.close({}, modalType);
            }));
        });

    });
})();