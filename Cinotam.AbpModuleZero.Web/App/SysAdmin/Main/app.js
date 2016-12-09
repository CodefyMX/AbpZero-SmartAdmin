(function () {
    'use strict';
    var webConst = {
        contentFolder: '/App/SysAdmin/Main/modules/web/',
        datatablesLangConfig: {
            "decimal": "",
            "emptyTable": App.localize("NoDataAvailableInTable"), //"No hay datos disponibles en la tabla"
            "info": App.localize("ShowingStartToEnd"),  // "Mostrando _START_ a _END_ de _TOTAL_ entradas",
            "infoEmpty": App.localize("ZeroEntries"),// "Mostrando 0 a 0 de 0 entradas",
            "infoFiltered": "",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": App.localize("ShowEntries"), // "Mostrar _MENU_ entradas",
            "loadingRecords": App.localize("Loading"), //"Cargando...",
            "processing": App.localize("Processing"), //"Procesando...",
            "search": App.localize("Search"),// "Buscar:",
            "zeroRecords": App.localize("NoCoincidences"),// "No hay coincidencias",
            "paginate": {
                "first": App.localize("PaginateFirst"),
                "last": App.localize("PaginateLast"),
                "next": App.localize("PaginateNext"),
                "previous": App.localize("PaginateBefore")
            },
            "aria": {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            }
        }
    }
    var app = angular.module('app', [
        'ngAnimate',
        'ngSanitize',
        'app.core',
        'app.web',
        'ui.bootstrap',
        'ngJsTree',
        'datatables',
        'datatables.bootstrap',
        'abp'
    ]).constant('WebConst', webConst);
})();
