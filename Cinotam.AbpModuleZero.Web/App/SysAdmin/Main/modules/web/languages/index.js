(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.languages.index', LanguagesController);

    LanguagesController.$inject = [];
    function LanguagesController() {
        var vm = this;
        vm.instance = {};
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
        vm.url = '/AngularApi/Languages/LoadLanguages';
        vm.objFuncs = [
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.createEditTexts(' + data.Id + ')" ><i class="fa fa-edit"></i></a>';
                },
            },
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.delete(' + data.Id + ',&quot ' + data.Name + ' &quot)" ><i class="fa fa-trash"></i></a>';
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