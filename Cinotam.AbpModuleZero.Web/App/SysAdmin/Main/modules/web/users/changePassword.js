(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.users.changePassword', ChangePasswordController);

    ChangePasswordController.$inject = ["$uibModalInstance", "items", "abp.services.app.user"];
    function ChangePasswordController($uibModalInstance, items, _userService) {
        var vm = this;
        vm.user = {
            newPassword: "",
            userId: items.userId,
            confirmPassword: ""
        };
        vm.userName = items.userName;
        vm.cancel = function () {
            $uibModalInstance.close();
        };
        vm.submit = function () {

            if (vm.user.newPassword != vm.user.confirmPassword) {
                abp.message.error(App.localize("PasswordsNotMatch"), App.localize("Error"));
            }
            else {
                _userService.changePasswordFromAdmin(vm.user).then(function () {
                    $uibModalInstance.close("passwordchanged");
                });
            }

        };
        ////////////////

        function activate() {
            //
        }
        activate();
    }
})();