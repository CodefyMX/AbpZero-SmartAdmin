(function() {
'use strict';

    angular
        .module('app.web')
        .controller('app.views.organizationUnits.users', OrganizationUnitsUsersController);

    OrganizationUnitsUsersController.$inject = ['$stateParams','abp.services.app.organizationUnits','$uibModal'];
    function OrganizationUnitsUsersController($stateParams,_orgUnits,modal) {
        var vm = this;
        vm.orgId = $stateParams.id;
        activate();
        ////////////////
        vm.removeUser = function(user,orgId){
            console.log(user);
            console.log(vm.orgId);
        }
        vm.openAddUserModal = function(){
            var modalInstance = modal.open({
                controller: 'app.views.organizationUnits.addUser as vm',
                templateUrl:'/App/SysAdmin/Main/modules/web/organizationUnits/adduser.cshtml',
                resolve:{
                    items:function(){
                        return {
                            orgUnitId:vm.orgId
                        }
                    }
                }
            });
            modalInstance.result.then(function(response){

            });
        }
        vm.empty = true;

        function activate() {
                vm.id = $stateParams.id;
                _orgUnits.getUsersFromOrganizationUnit(vm.id).then(function(response){
                    vm.orgUnitModel = response.data;
                    if(vm.orgUnitModel.users.length>0){
                        vm.empty = false;
                    }
                    else{
                        vm.empty = true;
                    }
                });
        }
    }
})();