(function () {
    'use strict';
    var isAuditLogGranted = abp.auth.isGranted('Pages.AuditLogs');
    if (isAuditLogGranted) {
        var columns = [
            {
                "data": "MethodName"
            },
            {
                "data": "ServiceName"
            },
            { "data": "UserName" },
            { "data": "ClientIpAddress" },
            {
                "data": "ExecutionTimeString"
            },
            {
                "data": "ExecutionDuration"
            },
            {
                "data": "BrowserInfo"
            }
        ];
        var columnDefinitions = [
            {
                className: "text-center",
                "render": function(data, type, row) {
                    return "<a data-modal class='btn btn-default btn-xs' href='/SysAdmin/AuditLogs/AuditLogDetail/" +
                        row.Id +
                        "' title=''><i class='fa fa-file-text-o'></i></a>";
                },
                targets: 7
            }
        ];
        var dataTablesConfig = new DatatablesConfig({
            Url:"/SysAdmin/AuditLogs/" + "LoadLogs",
            DisplayLength: 10,
            OnInitComplete:function () {
                var id = $("#Id").val();
                if (id != 0) {
                    window.modalInstance.open("/SysAdmin/AuditLogs/AuditLogDetail/" + id + "");
                }
            },
            Element:$("#auditLogsTable"),
            Columns:columns,
            ColumnDefinitions : columnDefinitions 
        });

        var auditLogListPage = {
            dataTablesConfig: dataTablesConfig
        }

        var $table = $("#auditLogsTable");

        var table = $table
        .DataTable(auditLogListPage.dataTablesConfig);
    }

})();