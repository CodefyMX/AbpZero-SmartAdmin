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

    var $tenantId;
    var $chrtFourth = "#7e9d3a";
    var $chrtError = "#f95f0b";
    var $logsChart = $('#logsChart');
    var isInitialized = false;
    var serieCount = 0;
    var updateInterval = 3000;
    var selectedOptionValue = 200;
    var $menu = $("#menu");
    var allToolTips = [];
    //Se esta haciendo un doble bind, el bind debe ser unico
    //El data index tambien es el mismo para ambos tooltips

    function getCorrectToolTipIndex(toolTips, item) {

        //toolTips[item.dataIndex]
        var seriesIndex = item.seriesIndex;

        var dataIndex = item.dataIndex;

        for (var i = 0; i < toolTips.length; i++) {
            if (i == dataIndex) {
                for (var j = 0; j < toolTips.length; j++) {
                    if (toolTips[j].SerieId == seriesIndex) {

                        return toolTips[j];
                    }
                }
            }

        }
        return undefined;
    }

    var buildToolTips = function (toolTips) {

        $logsChart.bind("plotclick", function (event, pos, item) {
            if (item) {
                var toolTip = getCorrectToolTipIndex(toolTips, item); //toolTips[item.dataIndex]; 
                
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

    }

    var chartData = [];
    var plot = {};
    var colorArray = [];
    var options = {};

    /**
     * 
     * @param {} dataArray 
     * @param {} toolTipsArray 
     * @returns {} 
     */
    var buildChart = function (data, color) {
        colorArray.push(color);



        var chartElementData =
        {
            data: data.data,
            label: data.label
        };


        chartData.push(chartElementData);

        if (!isInitialized) {

            options = {
                colors: colorArray,
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
                zoom: {
                    interactive: true
                },
                pan: {
                    interactive: true
                }
            }

            plot = $.plot($logsChart, [chartElementData], options);

            isInitialized = true;



            // show pan/zoom messages to illustrate events 

            $menu.bind("plotpan", function (event, plot) {
                var axes = plot.getAxes();
                $(".message").html("Panning to x: " + axes.xaxis.min.toFixed(2)
                + " &ndash; " + axes.xaxis.max.toFixed(2)
                + " and y: " + axes.yaxis.min.toFixed(2)
                + " &ndash; " + axes.yaxis.max.toFixed(2));
            });

            $menu.bind("plotzoom", function (event, plot) {
                var axes = plot.getAxes();
                $(".message").html("Zooming to x: " + axes.xaxis.min.toFixed(2)
                + " &ndash; " + axes.xaxis.max.toFixed(2)
                + " and y: " + axes.yaxis.min.toFixed(2)
                + " &ndash; " + axes.yaxis.max.toFixed(2));
            });





        } else {

            options.colors.push(color);

            plot.setData(chartData);
            plot.setupGrid();
            plot.draw();


        }

    }

    var getData;

    function initialize(_auditLogAppService) {

        getData = function (max, callback, code) {

            var $averageElement = $("#average");
            var $totalElement = $("#total");

            if (!$tenantId) {
                $tenantId = null;
            }
            var request = {
                Count: max,
                Code: code,
                TenantId:$tenantId

            }
            _auditLogAppService.getAuditLogTimes(request)
                .done(function (response) {

                    var data = [];
                    var dataToolTips = [];
                    var count = 1;
                    var element = $("#avgIndicator");
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
                            Id: element.id,
                            SerieId: serieCount
                        });
                        data.push([count, element.executionDuration]);
                        count = count + 1;
                    });

                    serieCount = serieCount + 1;

                    callback(data, dataToolTips);
                });
        }

        startView(selectedOptionValue);


    }

    function startView(total) {
        if ($logsChart.length) {
            if (isAuditLogGranted) {
                // setup control widget
                getData(total, function (data, toolTips) {

                    for (var i = 0; i < toolTips.length; i++) {
                        allToolTips.push(toolTips[i]);
                    }

                    var successData = {


                        data: data,
                        label: LSys("Success")
                    }

                    buildChart(successData, $chrtFourth);

                    getData(total, function (errorRData, errorRToolTips) {



                        var errorData = {


                            data: errorRData,
                            label: LSys("Error")
                        }

                        buildChart(errorData, $chrtError);
                        for (var i = 0; i < errorRToolTips.length; i++) {
                            allToolTips.push(errorRToolTips[i]);
                        }
                        buildToolTips(allToolTips);


                        update();


                    }, resultTypes.ONLY_ERROR);


                }, resultTypes.ONLY_SUCCESS);
            }

        }
    }

    function update() {
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
        var $startInterval = $("#start_interval");


        if ($startInterval.is(":checked")) {


            getData(selectedOptionValue, function (updatedData, updatedToolTips) {

                allToolTips = [];

                chartData = [];
                for (var i = 0; i < updatedToolTips.length; i++) {
                    allToolTips.push(updatedToolTips[i]);
                }

                var successData = {
                    data: updatedData,
                    label: LSys("Success")
                }
                buildChart(successData, $chrtFourth);
                getData(selectedOptionValue, function (updatedErrorData, updatedErrorToolTips) {


                    var errorData = {
                        data: updatedErrorData,
                        label: LSys("Error")
                    }

                    buildChart(errorData, $chrtError);
                    for (var i = 0; i < updatedErrorToolTips.length; i++) {
                        allToolTips.push(updatedErrorToolTips[i]);
                    }

                    buildToolTips(allToolTips);

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


    var logPageConfig = {
        initialize: initialize
    }

    $(document)
        .ready(function () {
            $tenantId = $("#tenantId").val();
            logPageConfig.initialize(_auditLogAppService);
        });
})();