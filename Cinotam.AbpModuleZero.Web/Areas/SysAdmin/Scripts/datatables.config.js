
var ColumnElement = function (renderFunction, className, targets) {
    this.render = renderFunction;
    this.className = className;
    this.targets = targets;
};
var DatatablesConfig = function (configuration) {
    var responsiveHelper;
    var serverSide = true;
    if (configuration.Ajax != undefined) {
        
        if (configuration.Ajax === false) {
             serverSide = false;
        }
    }

    var datatablesConfig = {
        "bServerSide": serverSide,
        "bPaginate": true,
        "sPaginationType": "full_numbers", // And its type.
        "iDisplayLength": configuration.DisplayLength,
        "ajax": configuration.Url,
        "autoWidth": true,
        "preDrawCallback": function () {
            // Initialize the responsive datatables helper once.
            if (!responsiveHelper) {
                responsiveHelper = new ResponsiveDatatablesHelper(configuration.Element, breakpointDefinition);
            }
        },
        "rowCallback": function (nRow) {
            responsiveHelper.createExpandIcon(nRow);
        },
        "drawCallback": function (oSettings) {
            responsiveHelper.respond();
        },
        language: window.dataTablesLang,

        columnDefs: configuration.ColumnDefinitions,
        initComplete: configuration.OnInitComplete,
        columns: configuration.Columns


    };

    return datatablesConfig;
};