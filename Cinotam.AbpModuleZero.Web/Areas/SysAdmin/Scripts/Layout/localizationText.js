
/**
 * 
 * Summary:
 * Some helpers for transaltions 
 */
var LSys = function (string) {
    return abp.localization.localize (string, "AbpModuleZero");
};

var LSmart = function (string) {
    return abp.localization.localize(string, "CinotamSmartAdmin");
};


(function () {
    window.dataTablesLang = {
        "decimal": "",
        "emptyTable": LSmart("NoDataAvailableInTable"), //"No hay datos disponibles en la tabla"
        "info": LSmart("ShowingStartToEnd"),  // "Mostrando _START_ a _END_ de _TOTAL_ entradas",
        "infoEmpty": LSmart("ZeroEntries"),// "Mostrando 0 a 0 de 0 entradas",
        "infoFiltered": "",
        "infoPostFix": "",
        "thousands": ",",
        "lengthMenu": LSmart("ShowEntries"), // "Mostrar _MENU_ entradas",
        "loadingRecords": LSmart("Loading"), //"Cargando...",
        "processing": LSmart("Processing"), //"Procesando...",
        "search": LSmart("Search"),// "Buscar:",
        "zeroRecords": LSmart("NoCoincidences"),// "No hay coincidencias",
        "paginate": {
            "first": LSmart("PaginateFirst"),
            "last": LSmart("PaginateLast"),
            "next": LSmart("PaginateNext"),
            "previous": LSmart("PaginateBefore")
        },
        "aria": {
            "sortAscending": ": activate to sort column ascending",
            "sortDescending": ": activate to sort column descending"
        }
    }
})();