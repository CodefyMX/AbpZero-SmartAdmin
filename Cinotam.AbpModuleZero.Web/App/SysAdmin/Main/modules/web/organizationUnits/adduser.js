(function() {
'use strict';

    angular
        .module('app.web')
        .controller('app.views.organizationUnits.addUser', AddUserController);

    AddUserController.$inject = ['items','$uibModalInstance','abp.services.app.organizationUnits'];
    function AddUserController(items,uibModalInstance,_orgUnits) {
        var vm = this;
        vm.cancel = function(){
            uibModalInstance.close();
        }
        vm.userId = 0;
        vm.click = function(id){
            vm.userId =id;

            _orgUnits.addUserToOrgUnit({
                UserId:vm.userId,
                OrgUnitId:items.orgUnitId
            }).then(function(){
                uibModalInstance.close("useradded");
            });
        }
        activate();
        ////////////////
        
        function activate() { }
    }
})();