(function () {
    var isAuditLogGranted = abp.auth.isGranted('Pages.AuditLogs');
    var currentValue;
    var newValue;
    var _auditLogAppService = abp.services.app.auditLogService;
    var resultTypes = {

        ALL: 0,
        ONLY_ERROR: 1,
        ONLY_SUCCESS: 2

    }
    function initialize(_auditLogAppService) {

        var selectedOptionValue = 50;
        function getData(max, callback, code) {

            var $averageElement = $("#average");
            var $totalElement = $("#total");
            _auditLogAppService.getAuditLogTimes(max, code)
                .done(function (response) {

                    var data = [];
                    var dataToolTips = [];
                    var count = 1;
                    var element = $("#avgIndicator");
                    //if (currentValue == response.avgExecutionTime) {
                    //    console.log ("No changes");
                    //}
                    if (currentValue < response.avgExecutionTime) {
                        element.removeClass("fa fa-caret-down icon-color-good");
                        element.addClass("fa fa-caret-up icon-color-bad");
                    }
                    if (currentValue > response.avgExecutionTime) {
                        element.removeClass("fa fa-caret-up icon-color-bad");
                        element.addClass("fa fa-caret-down icon-color-good");
                    }
                    if (currentValue == undefined) {
                        currentValue = response.avgExecutionTime;
                    }
                    if (response.avgExecutionTime !== currentValue) {
                        currentValue = response.avgExecutionTime;
                    }
                    newValue = response.avgExecutionTime;
                    currentValue = response.avgExecutionTime;
                    $averageElement.text(newValue + " ms");
                    $totalElement.text(response.totalRequestsReceived);
                    response.auditLogTimeOutputDtos.forEach(function (element) {
                        dataToolTips.push({
                            MethodName: element.methodName,
                            Id: element.id
                        });
                        data.push([count, element.executionDuration]);
                        count = count + 1;
                    });
                    callback(data, dataToolTips);
                });
        }


        var $chrtFourth = "#7e9d3a";
        var $chrtError = "#f95f0b";
        var $logsChart = $('#logsChart');
        if ($logsChart.length) {
            if (isAuditLogGranted) {

                // setup control widget
                getData(50, function (data, toolTips) {


                    var successData = data;
                    getData(50,
                        function (dataErr, toolTipsErr) {

                            var updateInterval = 1000;
                            $logsChart.val(updateInterval).change(function () {
                                var v = $(this).val();
                                if (v && !isNaN(+v)) {
                                    updateInterval = +v;
                                    if (updateInterval < 1)
                                        updateInterval = 1;
                                    if (updateInterval > 2000)
                                        updateInterval = 2000;
                                    $(this).val("" + updateInterval);
                                }
                            });

                            // setup plot
                            var options = {
                                colors: [$chrtFourth, $chrtError],
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
                                    },                                    points: {
                                        show: true
                                    },
                                    shadowSize: 0

                                },
                                grid: {
                                    hoverable: true,
                                    clickable: true
                                }
                            };
                            var plot = $.plot($logsChart, [
                                {
                                    data: successData,
                                    label: 'Success'
                                },
                                {
                                    data: dataErr,
                                    label: 'Errors'
                                }

                            ], options);

                            $logsChart.bind("plotclick", function (event, pos, item) {

                                if (item) {
                                    var toolTip = toolTips[item.dataIndex];

                                    if (toolTip) {
                                        var id = toolTip.Id;
                                        window.modalInstance.open("/SysAdmin/AuditLogs/AuditLogDetail/" + id + "");
                                    }
                                    
                                }
                            });


                            $logsChart.bind("plotclick", function (event, pos, item) {

                                if (item) {
                                    var toolTip = toolTipsErr[item.dataIndex];
                                    if (toolTip) {
                                        var id = toolTip.Id;
                                        window.modalInstance.open("/SysAdmin/AuditLogs/AuditLogDetail/" + id + "");
                                    }
                                    
                                }
                            });


                            $("<div id='tooltip'></div>").css({
                                position: "absolute",
                                display: "none",
                                border: "1px solid #fdd",
                                padding: "2px",
                                "background-color": "#fee",
                                opacity: 0.80
                            }).appendTo("body");
                            var $toolTip = $("#tooltip");
                            $logsChart.bind("plothover", function (event, pos, item) {
                                if (item) {
                                    var toolTip = toolTips[item.dataIndex];

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
                            });

                            $logsChart.bind("plothover", function (event, pos, item) {
                                if (item) {
                                    var toolTip = toolTipsErr[item.dataIndex];


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
                            });
                            function update() {
                                var $startInterval = $("#start_interval");


                                if ($startInterval.is(":checked")) {


                                    getData(selectedOptionValue, function (updatedData, updatedToolTips) {



                                        getData(selectedOptionValue, function (updatedErrorData, updatedErrorToolTips) {

                                            data = [];
                                            data = updatedData;
                                            toolTips = [];
                                            toolTips = updatedToolTips;
                                            toolTipsErr = updatedErrorToolTips;
                                            plot.setData([data, updatedErrorData]);
                                            plot.setupGrid();
                                            plot.draw();
                                        }, resultTypes.ONLY_ERROR);

                                        setTimeout(update, updateInterval);

                                    }, resultTypes.ONLY_SUCCESS);
                                }
                                $startInterval
                                    .change(function () {
                                        if ($startInterval.is(":checked")) {
                                            setTimeout(update, 1000);
                                        }
                                    });

                            }

                            update();

                        }, resultTypes.ONLY_ERROR);






                }, resultTypes.ONLY_SUCCESS);
            }

        }
    }
    var logPageConfig = {
        initialize: initialize
    }

    $(document)
        .ready(function () {

            logPageConfig.initialize(_auditLogAppService);
        });
})();