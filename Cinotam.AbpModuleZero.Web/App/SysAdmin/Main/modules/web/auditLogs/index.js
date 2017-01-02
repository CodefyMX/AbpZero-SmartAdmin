(function() {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.auditLogs.index', AuditLogsController);

    AuditLogsController.$inject = ['$uibModal', 'WebConst', "abp.services.app.auditLogService"];
    function AuditLogsController($uibModal, WebConst, _auditLogService) {
        var vm = this;
        ///////Chart/////////
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
        vm.changeLogType = function(option) {
            requestModel.code = option;
            vm.update();
        }
        vm.selectVal = 50;
        activate(vm.chartConfigs.selectedOptionValue);
        vm.update = function() {
            requestModel.Count = vm.selectVal;
            activate();
        }
        vm.labels = [];
        ///////--Chart/////////
        vm.instance = {};
        vm.serverSide = true;
        vm.defaultSearchPropery = 'Name';
        vm.properties = [
            {
                Key: "MethodName",
                DisplayName: App.localize("MethodName")

            },
            {
                Key: "ServiceName",
                DisplayName: App.localize("ServiceName"),

            },
            {
                Key: "UserName",
                DisplayName: App.localize('UserName'),
            },
            {
                Key: "ClientIpAddress",
                DisplayName: App.localize('IP'),
                Responsive: true
            },
            {
                Key: "BrowserInfo",
                DisplayName: App.localize('BrowserInfo'),
            }
        ];
        vm.objFuncs = [
            {
                dom: function(data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.openModal(' + data.Id + ')" ><i class="fa fa-edit"></i></a>';
                }
            }
        ];
        vm.url = '/AngularApi/AuditLogs/LoadLogs';

        vm.reloadTable = function() {
            vm.instance.reloadData(function(data) {
            }, false);
        }

        vm.openModal = function(id) {
            var modalInstance = $uibModal.open({
                templateUrl: WebConst.contentFolder + 'dashboard/logDetails.cshtml',
                controller: 'app.views.dashboard.logDetails as vm',
                resolve: {
                    items: function() {
                        return id;
                    }
                }
            });
        }

        vm.flotData = [];
        function activate() {
            vm.flotData = [];
            if (!tenantId) tenantId = null;
            getChartData(function(data, labels) {
                var text = App.localize("Success");
                console.log(requestModel);
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


        vm.onHover = function(event, pos, item) {

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
        vm.onClick = function(event, pos, item) {

            if (item) {
                console.log(vm.toolTips);
                vm.hoveredItem = getCorrectToolTipIndex(vm.toolTips, item);
                vm.openModal(vm.hoveredItem.Id);
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
            _auditLogService.getAuditLogTimes(requestModel).then(function(response) {
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