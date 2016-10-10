
(function () {

    var table = $("#templatesTable")
.DataTable({
    "sPaginationType": "full_numbers", // And its type.
    "iDisplayLength": 10,
    "ajax": "/SysAdmin/Templates/" + "GetTemplates",
    "autoWidth": true,
    "preDrawCallback": function () {
        // Initialize the responsive datatables helper once.
        if (!responsiveHelper_dt_roles) {
            responsiveHelper_dt_roles = new
                ResponsiveDatatablesHelper($('#templatesTable'), breakpointDefinition);
        }
    },
    "rowCallback": function (nRow) {
        responsiveHelper_dt_roles.createExpandIcon(nRow);
    },
    columnDefs: [
        {
            className: "text-center",
            "render": function (data, type, row) {
                if (row.IsPartial) {
                    return "<label class='label label-primary'>" + LSys("IsPartial") + "</label>";
                } else {
                    return "<label class='label label-primary'>" + LSys("IsNotPartial") + "</label>";
                }
            },
            "targets": 2
        },
        {
            className: "text-center",
            "render": function (data, type, row) {
                if (row.IsDatabaseTemplate) {
                    return "<label class='label label-primary'>" + LSys("IsFromDatabase") + "</label>";
                } else {
                    return "<label class='label label-primary'>" + LSys("IsFromFileSystem") + "</label>";
                }
            },
            "targets": 3
        },
        {
            className: "text-center",
            "render": function (data, type, row) {
                if (row.IsDatabaseTemplate) {
                    var buttons = "<a  href='/SysAdmin/Templates/EditHtml/" +
                        row.Name +
                        "' class='btn btn-primary btn-xs'><i class='fa fa-code'></i></a>" +
                        "   <a data-modal href='/SysAdmin/Templates/AddCssRes/";
                    if (!row.IsPartial) {
                        buttons += row.Name +
                            "' class='btn btn-primary btn-xs'><i class='fa fa-css3'></i></a>";
                    } else {
                        buttons += row.Name +
                            "' class='btn btn-primary btn-xs disabled'><i class='fa fa-css3'></i></a>";
                    }
                    return buttons;
                } else {
                    return "<a class='btn btn-primary btn-xs disabled'><i class='fa fa-code'></i></a>  <a target='_blank' class='btn btn-primary btn-xs disabled'><i class='fa fa-css3'></i></a>";
                }
            },
            "targets": 4
        }
    ],
    "drawCallback": function (oSettings) {
        responsiveHelper_dt_roles.respond();
    },
    language: window.dataTablesLang,
    columns: [
        {
            "data": "Name"
        },
        {
            "data": "FileName"
        }
    ]
});



})();