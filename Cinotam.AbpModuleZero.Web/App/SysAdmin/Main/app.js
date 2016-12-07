(function () {
    'use strict';

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
    ]).constant('APP_CONFIG', window.appConfig);
})();
