(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.languageText.index', LanguageTextsController);

    LanguageTextsController.$inject = ['$stateParams'];
    function LanguageTextsController($stateParams) {
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
        vm.url = '/AngularApi/Languages/LoadLanguageTexts';
        vm.objFuncs = [
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.createEditTexts(&quot ' + data.Name + ' &quot)" ><i class="fa fa-edit"></i></a>';
                },
            }
        ]

        vm.reloadTable = function () {
            vm.instance.reloadData();
        }
        activate();

        ////////////////

        function activate() {
            vm.data.targetLang = $stateParams.targetLang;
        }
    }
})();