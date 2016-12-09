(function () {
    'use strict';
    window.webConst = {
         contentFolder:'/App/SysAdmin/Main/modules/web/',
         
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
    ]).constant('WebConst',window.webConst);
})();
