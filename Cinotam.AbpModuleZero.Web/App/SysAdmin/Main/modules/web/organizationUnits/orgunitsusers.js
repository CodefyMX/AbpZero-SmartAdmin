(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.organizationUnits.users', OrganizationUnitsUsersController);

    OrganizationUnitsUsersController.$inject = ['$stateParams', 'abp.services.app.organizationUnits', '$uibModal'];
    function OrganizationUnitsUsersController($stateParams, _orgUnits, modal) {
        var vm = this;
        vm.orgId = $stateParams.id;
        activate();
        ////////////////
        vm.removeUser = function (userId, orgId) {
            abp.message.confirm(App.localize("UserWillBeRemovedFromOrganizationUnit"),
                App.localize("ConfirmQuestion"),
                function(response) {
                    if (response) {
                        _orgUnits.removeUserFromOrganizationUnit({
                                UserId: userId,
                                OrgUnitId: vm.orgId
                            })
                            .then(function() {
                                abp.notify.success(App.localize("UserRemovedFromOrganizationUnit"), App.localize("Success"));
                                activate();
                            });
                    }
                });
        }
        vm.openAddUserModal = function () {
            var modalInstance = modal.open({
                controller: 'app.views.organizationUnits.addUser as vm',
                templateUrl: '/App/SysAdmin/Main/modules/web/organizationUnits/adduser.cshtml',
                resolve: {
                    items: function () {
                        return {
                            orgUnitId: vm.orgId
                        }
                    }
                },
                size: 'lg'
            });
            modalInstance.result.then(function (response) {
                if (response === "useradded") {
                    abp.notify.success(App.localize("UserAdded"), App.localize("Success"));


                    activate();


                }
            });
        }
        vm.empty = true;

        function activate() {
            vm.id = $stateParams.id;
            _orgUnits.getUsersFromOrganizationUnit(vm.id).then(function (response) {
                vm.orgUnitModel = response.data;
                if (vm.orgUnitModel.users.length > 0) {
                    vm.empty = false;
                }
                else {
                    vm.empty = true;
                }
            });
        }
    }
})();