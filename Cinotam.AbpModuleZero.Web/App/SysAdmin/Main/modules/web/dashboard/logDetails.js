(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.dashboard.logDetails', LogDetailsController);

    LogDetailsController.$inject = ['$uibModalInstance', 'abp.services.app.auditLogService', 'items'];
    function LogDetailsController($uibModalInstance, _logService, items) {
        var vm = this;
        vm.logDetails = {};
        vm.cancel = function () {
            $uibModalInstance.close();
        }
        activate();
        var hasException = function (ex) {
            if (ex === "" || ex == null) {
                return false;
            }
            else {
                return true;
            }
        }
        vm.showException = false;
        vm.toggleException = function () {
            if (vm.showException) vm.showException = false;
            else vm.showException = true;
        }
        vm.hasException = false;
        ////////////////

        function activate() {

            _logService.getAuditLogDetails(items)
                .then(function (response) {
                    vm.logDetails = response.data;
                    vm.hasException = hasException(vm.logDetails.exception);
                });
        }
    }
})();