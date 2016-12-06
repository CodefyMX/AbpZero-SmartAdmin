(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.dashboard.index', DashboardController);

    DashboardController.$inject = ["abp.services.app.auditLogService", "$uibModal"];
    function DashboardController(_auditLogService, modal) {
        var vm = this;
        vm.name = "Hey";

        var chartConfigs = {
            updateInterval: 3000,
            selectedOptionValue: 50
        }

        vm.selectVal = 50;

        var tenantId;

        activate(chartConfigs.selectedOptionValue);

        ////////////////
        vm.chartData = {
            auditLogTimeOutputDtos: [],
            avgExecutionTime: "",
            totalRequestsReceived: 0
        };
        vm.update = function (d) {
            activate(vm.selectVal);
        }
        vm.chartDataFormatedDtos = [[]];
        vm.labels = [];
        vm.onClick = function (data) {

            if (data[0]) {
                var index = data[0]._index;
                var id = getIdFromData(index);
                if (data.length > 0) {
                    var modalInstance = modal.open({
                        templateUrl: '/App/SysAdmin/Main/modules/web/dashboard/logDetails.cshtml',
                        controller: 'app.views.dashboard.logDetails as vm',
                        resolve: {
                            items: function () {
                                return id;
                            }
                        }
                    });
                }
            }


            

        }

        function getIdFromData(index) {
            console.log(vm.chartData.auditLogTimeOutputDtos);
            for (var i = 0; i < vm.chartData.auditLogTimeOutputDtos.length; i++) {
                if (index === i) {
                    return vm.chartData.auditLogTimeOutputDtos[index].id;
                }
            }
            throw exception;
        }

        function activate(val) {
            if (!tenantId) tenantId = null;
            var requestModel = {
                Count: val,
                Code: 2,
                TenantId: tenantId
            }


            vm.chartOptions = {
                responsive: true,
                maintainAspectRatio: true,
                scales: {
                    xAxes: [
                        {
                            display: false
                        }
                    ]
                }
            }
            vm.chartDataFormatedDtos = [[]];
            vm.labels = [];
            _auditLogService.getAuditLogTimes(requestModel).then(function (response) {

                vm.chartData = response.data;
                for (var i = 0; i < vm.chartData.auditLogTimeOutputDtos.length; i++) {

                    vm.labels.push(i);

                    var elm = vm.chartData.auditLogTimeOutputDtos[i];

                    vm.chartDataFormatedDtos[0].push(elm.executionDuration);

                }
            });

        }
    }
})();