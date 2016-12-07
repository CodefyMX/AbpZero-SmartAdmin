(function() {
'use strict';

    angular
        .module('app.web')
        .controller('app.views.organizationUnits.create', CreateOrgUnitController);

    CreateOrgUnitController.$inject = ['$uibModalInstance','abp.services.app.organizationUnits','items','$scope'];
    function CreateOrgUnitController($uibModalInstance,_organizationUnits,params,$scope) {
        var vm = this;

        vm.orgUnit = {
            displayName:'',
            id:'',
            parentId:'',
            code:''
        }
        
        vm.cancel = function(){
            $uibModalInstance.close();
        };

        vm.submit = function() {
            abp.ui.setBusy();
            _organizationUnits.createOrEditOrgUnit(vm.orgUnit).then(function(){
                abp.ui.clearBusy();
                $uibModalInstance.close("created");
            });

        }
        activate();

        ////////////////

        function activate() { 
            console.log(params.id);
           if(params.id){
               _organizationUnits.getOrganizationUnitForEdit(params.id).then(function(response){

                   vm.orgUnit = response.data;
               })
           }
           else{
               if(params.parentId){
                   vm.orgUnit.parentId = params.parentId;
               }
           }

        }
    }
})();