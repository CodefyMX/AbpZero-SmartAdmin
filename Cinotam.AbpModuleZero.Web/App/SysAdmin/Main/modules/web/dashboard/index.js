(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.dashboard.index', DashboardController);

    DashboardController.$inject = ["abp.services.app.auditLogService", "$uibModal", '$interval', 'WebConst'];
    function DashboardController(_auditLogService, modal, $interval, webConst) {
        var vm = this;
        var serieCount = 0;
        vm.chartConfigs = {
            updateInterval: 3000,
            selectedOptionValue: vm.selectVal
        }
        vm.myChartOptions = {
            series: {
                lines: {
                    show: true,
                    lineWidth: 1,
                    fill: true,
                    fillColor: {
                        colors: [{
                            opacity: 0.1
                        }, {
                            opacity: 0.15
                        }]
                    }
                },
                points: {
                    show: true
                },

                shadowSize: 0

            },
            xaxis: {
                mode: "categories"
            },
            grid: {
                hoverable: true,
                clickable: true
            },
        };
        vm.toolTips = [];
        var tenantId;
        var requestModel = {
            Count: vm.chartConfigs.selectedOptionValue,
            Code: 2,
            TenantId: tenantId
        }
        vm.changeLogType = function (option) {
            requestModel.Code = option;
            vm.update();
        }
        vm.selectVal = 50;
        activate(vm.chartConfigs.selectedOptionValue);

        ////////////////
        vm.chartData = {
            auditLogTimeOutputDtos: [],
            avgExecutionTime: "",
            totalRequestsReceived: 0
        };
        vm.update = function () {
            requestModel.Count = vm.selectVal;
            activate();
        }
        vm.chartDataFormatedDtos = [[]];
        vm.labels = [];
        var openModal = function (id) {
            var modalInstance = modal.open({
                templateUrl: webConst.contentFolder + 'dashboard/logDetails.cshtml',
                controller: 'app.views.dashboard.logDetails as vm',
                resolve: {
                    items: function () {
                        return id;
                    }
                }
            });
        }

        vm.liveUpdate = false;
        vm.liveUpdateText = "Toggle live update";
        vm.toggleLiveUpdate = function () {
            if (vm.liveUpdate == false) vm.liveUpdate = true;
            else vm.liveUpdate = false;
            liveUpdate(vm.selectVal);
        }
        vm.flotData = [];
        function activate() {
            vm.flotData = [];
            if (!tenantId) tenantId = null;
            getChartData(function (data, labels) {
                var text = App.localize("Success");
                if (requestModel.Code != 2) {
                    text = App.localize("Error");
                }
                vm.toolTips = labels;
                vm.flotData.push({
                    data: data,
                    label: text
                });
            });
        }
        vm.hoveredItem = {};


        $("<div id='tooltip'></div>").css({
            position: "absolute",
            display: "none",
            border: "1px solid #fdd",
            padding: "2px",
            "background-color": "#fee",
            opacity: 0.80
        }).appendTo("body");
        var $toolTip = $("#tooltip");


        vm.onHover = function (event, pos, item) {

            if (item) {

                var toolTip = vm.toolTips[item.dataIndex];

                if (toolTip) {
                    var txt = toolTip.MethodName;
                    var value = item.datapoint[1];
                    var color = "";
                    if (value > 500) {
                        color = "#ffca28";
                    }
                    if (value <= 500) {
                        color = "#827717";
                    }
                    if (value > 1000) {
                        color = "#e53935";
                    }
                    var message = "Method name: " + txt + ", Execution time: <a style='color:" + color + "'> " + value + " ms<a>";
                    $toolTip
                        .html(message)
                        .css({ top: item.pageY + 5, left: item.pageX + 5 })
                        .fadeIn(200);
                }

            } else {
                $toolTip.hide();
            }

        }
        vm.onClick = function (event, pos, item) {

            if (item) {
                console.log(vm.toolTips);
                vm.hoveredItem = getCorrectToolTipIndex(vm.toolTips, item);
                openModal(vm.hoveredItem.Id);
            }
        }
        function getCorrectToolTipIndex(toolTips, item) {

            //toolTips[item.dataIndex]
            var seriesIndex = item.seriesIndex;

            var dataIndex = item.dataIndex;

            for (var i = 0; i < toolTips.length; i++) {

                if (i == dataIndex) {
                    for (var j = 0; j < toolTips.length; j++) {
                        if (toolTips[j].SerieId == seriesIndex) {
                            return toolTips[i];
                        }
                    }
                }
            }
            return undefined;
        }

        function getChartData(callback) {

            var florSeriesData = [];
            var toolTips = [];
            _auditLogService.getAuditLogTimes(requestModel).then(function (response) {
                vm.chartData = response.data;
                var count = 1;
                for (var i = 0; i < vm.chartData.auditLogTimeOutputDtos.length; i++) {
                    var elm = vm.chartData.auditLogTimeOutputDtos[i];
                    //Flot chart
                    florSeriesData.push([count, elm.executionDuration]);
                    toolTips.push({
                        MethodName: elm.methodName,
                        Id: elm.id,
                        SerieId: serieCount
                    });
                    count = count + 1;
                }
                //Number of series available
                // serieCount = serieCount + 1;
                //Flot chart
                callback(florSeriesData, toolTips);
            });
        }
    }
})();