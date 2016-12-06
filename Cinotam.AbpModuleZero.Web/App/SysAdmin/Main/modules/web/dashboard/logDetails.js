(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.dashboard.logDetails', LogDetailsController);

    LogDetailsController.$inject = ['$uibModalInstance', 'abp.services.app.auditLogService','items'];
    function LogDetailsController($uibModalInstance, _logService, items) {
        var vm = this;
        vm.logDetails = {};
        vm.cancel = function () {
            $uibModalInstance.close();
        }
        activate();

        ////////////////

        function activate() {

            _logService.getAuditLogDetails(items)
                .then(function (response) {
                    console.log(response.data);
                    vm.logDetails = response.data;
                    
                });

        }
    }
})();