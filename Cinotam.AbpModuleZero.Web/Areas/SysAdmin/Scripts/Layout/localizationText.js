
/**
 * 
 * Summary:
 * Some helpers for transaltions 
 */
var LSys = function (string) {
    return abp.localization.localize (string, "AbpModuleZero");
};

(function () {
    window.dataTablesLang = {
        "decimal": "",
        "emptyTable": LSys("NoDataAvailableInTable"), //"No hay datos disponibles en la tabla"
        "info": LSys("ShowingStartToEnd"),  // "Mostrando _START_ a _END_ de _TOTAL_ entradas",
        "infoEmpty": LSys("ZeroEntries"),// "Mostrando 0 a 0 de 0 entradas",
        "infoFiltered": "",
        "infoPostFix": "",
        "thousands": ",",
        "lengthMenu": LSys("ShowEntries"), // "Mostrar _MENU_ entradas",
        "loadingRecords": LSys("Loading"), //"Cargando...",
        "processing": LSys("Processing"), //"Procesando...",
        "search": LSys("Search"),// "Buscar:",
        "zeroRecords": LSys("NoCoincidences"),// "No hay coincidencias",
        "paginate": {
            "first": LSys("PaginateFirst"),
            "last": LSys("PaginateLast"),
            "next": LSys("PaginateNext"),
            "previous": LSys("PaginateBefore")
        },
        "aria": {
            "sortAscending": ": activate to sort column ascending",
            "sortDescending": ": activate to sort column descending"
        }
    }
})();