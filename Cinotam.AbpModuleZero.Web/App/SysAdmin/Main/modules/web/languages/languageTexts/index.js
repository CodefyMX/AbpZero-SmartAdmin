(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.languageText.index', LanguageTextsController);

    LanguageTextsController.$inject = ['$stateParams', '$uibModal', 'WebConst', 'abp.services.app.language'];
    function LanguageTextsController($stateParams, $uibModal, WebConst, _languageService) {
        var vm = this;
        vm.selectedSource = {
            name: 'AbpModuleZero'
        }
        vm.selectedLang = {
            name: 'en'
        }
        vm.selectedTargetLang = {
            name: 'en'
        }
        vm.data = {
            source: 'AbpModuleZero',
            sourceLang: 'en', // Always available
            targetLang: 'en'
        }
        vm.updateTable = function () {
            vm.data.source = vm.selectedSource.name;
            vm.data.sourceLang = vm.selectedLang.name;
            vm.data.targetLang = vm.selectedTargetLang.name;
            console.log(vm.data);
            vm.instance.updateRequest(vm.data);
            // vm.reloadTable();
        }
        vm.targetLangs = abp.localization.languages;
        vm.sourceLangs = abp.localization.languages;
        vm.sources = abp.localization.sources;
        vm.serverSide = false;
        vm.instance = {};
        vm.defaultSearchPropery = 'Key';
        vm.properties = [
            {
                Key: "Key",
                DisplayName: App.localize("Key"),
                onlyHolder: true
            },
            {
                Key: "SourceValue",
                DisplayName: App.localize("Source"),
                onlyHolder: true,
            },
            {
                Key: "TargetValue",
                DisplayName: App.localize('Target'),
                onlyHolder: true
            },
        ];
        vm.editTexts = function (name, source, targetValue) {
            var modalInstance = $uibModal.open({
                templateUrl: WebConst.contentFolder + 'languages/languageTexts/editText.cshtml',
                controller: 'app.views.languageTexts.editText as vm',
                resolve: {
                    items: function () {
                        return {
                            key: name,
                            source: source,
                            value: targetValue,
                            languageName: vm.selectedTargetLang.name
                        }
                    }
                }
            });
            modalInstance.result.then(function (response) {
                if (response == 'textchanged') {
                    vm.reloadTable(); // until i find a way to update only the selected row
                }
            });
        }
        vm.updateFromXml = function () {
            var lang = vm.selectedLang.name;
            var sourceDic = vm.selectedSource.name;
            abp.message.confirm(App.localize("LanguageTextsWillBeUpdated"), App.localize("ConfirmQuestion"), function (response) {
                if (response) {
                    _languageService.updateLanguageFromXml(lang, sourceDic, true).then(function () {
                        vm.reloadTable();
                    })
                }
            });
        }
        vm.url = '/AngularApi/Languages/LoadLanguageTexts';
        vm.objFuncs = [
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.editTexts(&quot ' + full.Key + ' &quot,&quot ' + full.Source + ' &quot,&quot ' + full.TargetValue + ' &quot)" ><i class="fa fa-edit"></i></a>';
                },
            }
        ]

        vm.reloadTable = function (callback) {
            vm.instance.reloadData();
            if (!callback) {
                callback = function () { }
            }
            callback();
        }
        activate();
        vm.instanceReady = function () {
            vm.updateTable();
        }
        ////////////////

        function activate() {
            vm.selectedTargetLang.name = $stateParams.targetLang;
        }
    }
})();