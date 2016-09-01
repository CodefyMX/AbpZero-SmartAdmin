(function () {
    'use strict';
    var isAuditLogGranted = abp.auth.isGranted('Pages.AuditLogs');

    if (isAuditLogGranted) {
        var table = $("#auditLogsTable")
        .DataTable({
            "bServerSide": true,
            "bPaginate": true,
            "sPaginationType": "full_numbers", // And its type.
            "iDisplayLength": 10,
            "ajax": "/SysAdmin/AuditLogs/" + "LoadLogs",
            "autoWidth": true,
            "preDrawCallback": function () {
                // Initialize the responsive datatables helper once.
                if (!responsiveHelper_dt_roles) {
                    responsiveHelper_dt_roles = new
                        ResponsiveDatatablesHelper($('#auditLogsTable'), breakpointDefinition);
                }
            },
            "rowCallback": function (nRow) {
                responsiveHelper_dt_roles.createExpandIcon(nRow);
            },
            "drawCallback": function (oSettings) {
                responsiveHelper_dt_roles.respond();
            },
            language: window.dataTablesLang,
            columnDefs: [
                {
                    className: "text-center",
                    "render": function (data, type, row) {
                        return "<a data-modal class='btn btn-default btn-xs' href='/SysAdmin/AuditLogs/AuditLogDetail/" + row.Id + "' title=''><i class='fa fa-file-text-o'></i></a>";
                    },
                    targets: 7
                }
            ],
            initComplete: function () {
                var id = $("#Id").val();


                if (id != 0) {
                    window.modalInstance.open("/SysAdmin/AuditLogs/AuditLogDetail/" + id + "");
                }
            },
            columns: [
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


            ]
        });
    }

})();