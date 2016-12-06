(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.dashboard.index', DashboardController);

    DashboardController.$inject = ["abp.services.app.auditLogService", "$uibModal", '$interval'];
    function DashboardController(_auditLogService, modal, $interval) {
        var vm = this;
        var errorColor = [
            {
                pointBackgroundColor: "rgba(159,204,0, 1)",
                backgroundColor: 'rgb(255,202,40)',
            }
        ];
        var successColor = [
            {
                pointBackgroundColor: "rgba(159,204,0, 1)",
                backgroundColor: 'rgb(102,187,106)',
            }
        ];
        vm.chartConfigs = {
            updateInterval: 3000,
            selectedOptionValue: 50,
            color: successColor
        }
        vm.changeLogType = function (option) {
            requestModel.code = option;
            if (option == 1) {
                vm.chartConfigs.color = errorColor;
            }
            else {
                 vm.chartConfigs.color = successColor;
            }
            vm.update();
        }
        var requestModel = {
            Count: vm.chartConfigs.selectedOptionValue,
            Code: 2,
            TenantId: tenantId
        }
        vm.selectVal = 50;

        var tenantId;

        activate(vm.chartConfigs.selectedOptionValue);

        ////////////////
        vm.chartData = {
            auditLogTimeOutputDtos: [],
            avgExecutionTime: "",
            totalRequestsReceived: 0
        };
        vm.update = function () {
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

        vm.liveUpdate = false;
        vm.liveUpdateText = "Toggle live update";
        vm.toggleLiveUpdate = function () {
            if (vm.liveUpdate == false) vm.liveUpdate = true;
            else vm.liveUpdate = false;
            liveUpdate(vm.selectVal);
        }
        function getIdFromData(index) {
            console.log(vm.chartData.auditLogTimeOutputDtos);
            for (var i = 0; i < vm.chartData.auditLogTimeOutputDtos.length; i++) {
                if (index === i) {
                    return vm.chartData.auditLogTimeOutputDtos[index].id;
                }
            }
        }

        function activate(val) {
            if (!tenantId) tenantId = null;
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
                liveUpdate();
            });
    
        }
        //Todo
        function liveUpdate() {
            if (vm.liveUpdate) {
                $interval(function () {
                    activateLiveUpdate();
                }, vm.chartConfigs.updateInterval);
            }
        }

        //var firstElement;
        function activateLiveUpdate(){

            //  _auditLogService.getAuditLogTimes(requestModel).then(function (response) {
            //     if(firstElement == 0){
            //         firstElement = getIdFromData(0);
            //     }
            //     if(response.data.auditLogTimeOutputDtos[0].id != firstElement) {
            //         console.log("Updating");
            //         vm.labels.push(vm.labels.length+1);
            //         var elm = response.data.auditLogTimeOutputDtos[0];
            //         vm.chartDataFormatedDtos[0].push(elm.executionDuration);
            //         firstElement = response.data.auditLogTimeOutputDtos[0].id;
            //     }
            // });
        }
    }
})();