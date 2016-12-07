(function() {
'use strict';

    angular
        .module('app.web')
        .controller('app.views.organizationUnits.addUser', AddUserController);

    AddUserController.$inject = ['items','$uibModalInstance'];
    function AddUserController(items,uibModalInstance) {
        var vm = this;
        vm.cancel = function(){
            uibModalInstance.close();
        }

        activate();
        console.log(items);
        ////////////////

        function activate() { }
    }
})();