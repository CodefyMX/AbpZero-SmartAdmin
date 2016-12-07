(function() {
'use strict';

    angular
        .module('app.web')
        .controller('app.views.organizationUnits.users', OrganizationUnitsUsersController);

    OrganizationUnitsUsersController.$inject = ['$stateParams','abp.services.app.organizationUnits'];
    function OrganizationUnitsUsersController($stateParams,_orgUnits) {
        var vm = this;
        vm.users = [];
        vm.orgId = $stateParams.id;
        activate();
        ////////////////
        vm.removeUser = function(user,orgId){
            console.log(user);
            console.log(vm.orgId);
        }
        function activate() {
                vm.id = $stateParams.id;
                _orgUnits.getUsersFromOrganizationUnit(vm.id).then(function(response){
                    vm.orgUnitModel = response.data;
                });
        }
    }
})();