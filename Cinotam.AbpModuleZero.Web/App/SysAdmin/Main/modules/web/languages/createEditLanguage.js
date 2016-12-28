(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.languages.createEditLanguage', CreateEditLanguageController);

    CreateEditLanguageController.$inject = ['$uibModalInstance', 'abp.services.app.language'];
    function CreateEditLanguageController($uibModalInstance, _languageService) {
        var vm = this;
        vm.language = {};
        vm.langName = 'es';
        vm.langIcon = 'famfamfam-flag-es';
        vm.submit = function () {
            var textVal = $('#selectName option:selected').text();
            var lang = textVal.substring(0, (textVal.indexOf("(") - 1))
            var clean = lang.replace(/ /g, '').replace(/[\r\n]/g, '');;
            vm.language = {
                LangCode: vm.langName,
                Icon: vm.langIcon,
                DisplayName: clean
            }
            _languageService.addLanguage(vm.language).then(function () {
                $uibModalInstance.close('ok');
            });
        }
        vm.cancel = function () {
            $uibModalInstance.close();
        }
        activate();

        ////////////////

        function activate() { }
    }
})();