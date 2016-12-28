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
        vm.data = {
            source: 'AbpModuleZero',
            sourceLang: 'en', // Always available
            targetLang: ''
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
        // vm.colDefs = [
        //     {
        //         render: function (data, type, row) {
        //             return ''
        //         },
        //         target: 0
        //     }
        // ]
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
            vm.instance.reloadData(function (data) {
            }, false);
        }

        activate();

        ////////////////

        function activate() {
            vm.data.targetLang = $stateParams.targetLang;
        }
    }
})();