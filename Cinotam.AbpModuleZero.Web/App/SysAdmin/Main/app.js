(function () {
    'use strict';

    var app = angular.module('app', [
        'ngAnimate',
        'ngSanitize',
        'app.core',
        'app.web',
        'ui.bootstrap',
        'ngJsTree',
        'abp'
    ]).constant('APP_CONFIG', window.appConfig);
})();
