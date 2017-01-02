(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.languageTexts.editText', EditTextController);

    EditTextController.$inject = ['$uibModalInstance', 'items', 'abp.services.app.language'];
    function EditTextController($uibModalInstance, items, _languageService) {
        var vm = this;

        vm.cancel = function () {
            $uibModalInstance.close();
        }
        vm.submit = function () {
            _languageService.addEditLocalizationText(vm.text).then(function () {
                $uibModalInstance.close('textchanged');
            });
        }
        vm.text = {}



        activate();

        ////////////////

        function activate() {
            var cleanName = items.key.replace(/ /g, '');
            var cleanSource = items.source.replace(/ /g, '');
            var cleanLang = items.languageName;
            vm.text.key = cleanName;
            vm.text.source = cleanSource;
            vm.text.languageName = cleanLang;
            vm.text.value = items.value.trim();
            console.log(vm.text);
        }
    }
})();