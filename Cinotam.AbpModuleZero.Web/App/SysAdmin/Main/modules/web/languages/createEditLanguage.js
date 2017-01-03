(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.languages.createEditLanguage', CreateEditLanguageController);

    CreateEditLanguageController.$inject = ['$uibModalInstance', 'abp.services.app.language'];
    function CreateEditLanguageController($uibModalInstance, _languageService) {
        var vm = this;
        vm.lang = {
            LangCode: 'es-ES',
            DisplayName: 'Spanish',
            Icon: 'famfamfam-flag-es'
        };
        vm.supportedLanguages = [
            {
                LangCode: 'es-ES',
                DisplayName: 'Spanish',
                Icon: 'famfamfam-flag-es'
            },
            {
                LangCode: 'es-MX',
                DisplayName: 'Spanish MX',
                Icon: 'famfamfam-flag-mx'
            }
        ]
        vm.submit = function () {
            _languageService.addLanguage(vm.lang).then(function () {
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