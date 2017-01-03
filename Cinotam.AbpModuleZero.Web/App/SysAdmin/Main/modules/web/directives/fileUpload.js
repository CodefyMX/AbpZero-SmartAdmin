(function () {
    'use strict';
    //Based in https://www.tutorialspoint.com/angularjs/angularjs_upload_file.htm
    angular
        .module('app.web')
        .directive('fileModel', Directive);

    Directive.$inject = ['$parse'];
    function Directive($parse) {
        var directive = {
            link: link,
            restrict: 'A'
        };
        return directive;
        function link(scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;
            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    }
})();