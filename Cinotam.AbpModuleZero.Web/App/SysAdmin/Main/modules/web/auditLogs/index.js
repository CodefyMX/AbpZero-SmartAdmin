(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.auditLogs.index', AuditLogsController);

    AuditLogsController.$inject = [];
    function AuditLogsController() {
        var vm = this;



        var d2 = [[0, 3]];
        vm.myData = [];
        vm.myChartOptions = {

        };
        ////////////////

        function activate() {
            vm.myData.push(d2);
        }
        
        activate();
    }
})();