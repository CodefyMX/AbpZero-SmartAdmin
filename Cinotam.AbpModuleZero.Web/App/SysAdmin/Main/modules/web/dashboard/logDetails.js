(function() {
'use strict';

    angular
        .module('app.web')
        .controller('app.views.dashboard.logDetails', LogDetailsController);

    LogDetailsController.$inject = ['$uibModalInstance'];
    function LogDetailsController($uibModalInstance) {
        var vm = this;
        
        vm.cancel = function(){
                $uibModalInstance.close();
        }
        activate();

        ////////////////

        function activate() { }
    }
})();