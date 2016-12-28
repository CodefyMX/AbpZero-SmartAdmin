(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.languages.createEditLanguage', CreateEditLanguageController);

    CreateEditLanguageController.$inject = [];
    function CreateEditLanguageController() {
        var vm = this;


        activate();

        ////////////////

        function activate() { }
    }
})();