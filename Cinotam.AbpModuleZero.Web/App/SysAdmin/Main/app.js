(function () {
    'use strict';

    var app = angular.module('app', [
        'ngAnimate',
        'ngSanitize',
        'app.core',
        'app.web',
        'ui.bootstrap',
        'abp'
    ]).constant('APP_CONFIG', window.appConfig);
})();
