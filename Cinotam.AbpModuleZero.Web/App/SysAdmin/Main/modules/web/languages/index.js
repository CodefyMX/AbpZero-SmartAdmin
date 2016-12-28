(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.languages.index', LanguagesController);

    LanguagesController.$inject = ['$state', '$uibModal', 'WebConst', 'abp.services.app.language'];
    function LanguagesController($state, $uibModal, WebConst, _languageService) {
        var vm = this;
        vm.instance = {};
        vm.serverSide = true;
        vm.defaultSearchPropery = 'Name';
        vm.properties = [
            {
                Key: "Id",
                DisplayName: "Id",
                Hidden: true,
            },
            {
                Key: "Name",
                DisplayName: App.localize("Name")
            },
            {
                Key: "CreationTimeString",
                DisplayName: App.localize("CreationTime")
            },
            {
                Key: "Type",
                DisplayName: App.localize('Type'),
                onlyHolder: true,
            },
        ];
        vm.createEditLanguage = function () {
            var modalInstance = $uibModal.open({
                templateUrl: WebConst.contentFolder + 'languages/createEditLanguage.cshtml',
                controller: 'app.views.languages.createEditLanguage as vm'
            });
            modalInstance.result.then(function (response) {
                if (response == 'ok') {
                    abp.notify.success(App.localize("LanguageCreated"), App.localize("Success"));
                    vm.reloadTable();
                }
            });
        }
        vm.colDefs = [
            {
                render: function (data, type, row) {
                    if (row.IsStatic) {
                        return "<span class='label label-default'>" + App.localize("Static") + "</span>";
                    } else {
                        return "<span class='label label-primary'>" + App.localize("NoStatic") + "</span>";
                    }
                },
                target: 4
            },
            {
                render: function (data, type, row) {
                    return "<i class=" + row.Icon + "></i> " + row.DisplayName;
                },
                target: 2
            }
        ]
        vm.createEditTexts = function (targetLang) {
            var clean = targetLang.replace(/ /g, '');
            $state.go('LanguageTexts', { targetLang: clean });
        }
        vm.delete = function (code) {
            var clean = code.replace(/ /g, '');
            var message = abp.utils.formatString(App.localize("DeleteLanguageMessage"), clean);
            abp.message.confirm(message, App.localize("ConfirmQuestion"), function (response) {
                if (response) {
                    _languageService.deleteLanguage(clean).then(function () {
                        abp.notify.warn("Lenguaje [" + clean + "] eliminado", App.localize('Success'));
                        vm.reloadTable();
                    });
                }
            });
        }
        vm.url = '/AngularApi/Languages/LoadLanguages';
        vm.objFuncs = [
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.createEditTexts(&quot ' + data.Name + ' &quot)" ><i class="fa fa-edit"></i></a>';
                },
            },
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.delete(&quot ' + data.Name + ' &quot)" ><i class="fa fa-trash"></i></a>';
                },
            },
        ]

        vm.reloadTable = function () {
            vm.instance.reloadData(function (data) {
            }, false);
        }
        activate();

        ////////////////

        function activate() { }
    }
})();