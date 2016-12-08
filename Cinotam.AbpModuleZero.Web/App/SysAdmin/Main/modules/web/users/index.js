(function() {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.users.index', IndexController);

    IndexController.$inject = [];
    function IndexController() {
        var vm = this;

        vm.properties = [
            {
                Key: "Id",
                DisplayName: "Id",
                Hidden: true,
            },
            {
                Key: "UserName",
                DisplayName: "User name"
            },
            {
                Key: "EmailAddress",
                DisplayName: "Email"
            }
        ]
        function edit() {
            console.log("Edit");
        }
        activate();
        vm.onSelected = function() {
            console.log("Ok");
        }

        vm.events = [
            {
                name: 'edit',
                dom: action,
                event: edit
            }
        ]
        function action(data, type, full, meta) {
            return '<a class="btn btn-default btn-xs" ng-click="vm.Edit(' + data.Id + ')" ><i class="fa fa-edit"></i></a> <a class="btn btn-danger btn-xs" ng-click="vm.Delete(' + data.Id + ')" ><i class="fa fa-times"></i></a> '
        }


        ////////////////

        function activate() {
        }
    }
})();