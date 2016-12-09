(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.users.createEdit', CreateEditController);

    CreateEditController.$inject = ['$uibModalInstance', 'items', '$scope', 'abp.services.app.user'];
    function CreateEditController($uibModalInstance, items, $scope, _usersService) {
        var vm = this;
        vm.user = {
            id: 0
        };
        activate();
        vm.cancel = function () {
            $uibModalInstance.close();
        }
        vm.submit = function (form) {

            _usersService.createUser(vm.user).then(function () {
                $uibModalInstance.close("userCreated");
            });
        }
        ////////////////

        function activate() {
            if (items.userId) {
                abp.ui.setBusy();
                _usersService.getUserForEdit(items.userId).then(function (response) {
                    vm.user = response.data;
                    abp.ui.clearBusy();
                })
            }
        }
    }
})();