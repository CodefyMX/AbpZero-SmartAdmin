(function() {
    'use strict';

    angular
        .module('app.core').run(appRun);
    appRun.$inject = ["routerHelper"];
    function appRun(routerHelper) {
    }
})();
