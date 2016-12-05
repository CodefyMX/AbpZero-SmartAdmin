(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.dashboard.index', DashboardController);

    DashboardController.$inject = ["abp.services.app.auditLogService"];
    function DashboardController(_auditLogService) {
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

        function activate(val) {
            if (!tenantId) tenantId = null;
            var requestModel = {
                Count: val,
                Code: 2,
                TenantId: tenantId
            }
            vm.onClick = function (data) {

                var index = data.index;


                console.log("Index position:",data[0]._index);

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

            _auditLogService.getAuditLogTimes(requestModel).then(function (response) {
                vm.chartData = response.data;
                for (var i = 0; i < vm.chartData.auditLogTimeOutputDtos.length; i++) {

                    vm.labels.push(i);

                    var elm = vm.chartData.auditLogTimeOutputDtos[i];

                    vm.chartDataFormatedDtos[0].push(elm.executionDuration);

                }


                console.log(vm.chartDataFormatedDtos);
                console.log(vm.labels);
            });

        }
    }
})();