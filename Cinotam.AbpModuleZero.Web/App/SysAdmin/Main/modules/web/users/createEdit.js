(function() {
'use strict';

    angular
        .module('app.web')
        .controller('app.views.users.createEdit', CreateEditController);

    CreateEditController.$inject = ['$uibModalInstance','items','$scope','abp.services.app.user'];
    function CreateEditController($uibModalInstance,items,$scope,_usersService) {
        var vm = this;
        vm.user = {};
        activate();
        vm.cancel = function (){
            $uibModalInstance.close("ok");
        }
        ////////////////

        function activate() { 
            console.log(items);
            if(items.userId){
                _usersService.getUserForEdit(items.userId).then(function(response){
                    vm.user = response.data;
                    console.log(vm.user);
                })
            }
        }
    }
})();