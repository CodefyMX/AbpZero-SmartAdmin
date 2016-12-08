(function() {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.users.index', IndexController);
    IndexController.$inject = [];
    function IndexController() {
        var vm = this;
        vm.instance = {};
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
                Key: "Name",
                DisplayName: "SurName"
            },
            {
                Key: "EmailAddress",
                DisplayName: "Email"
            },
            {
                Key:'CreationTimeString',
                DisplayName:'CreationTime'
            },
            {
                Key:'LastLoginTimeString',
                DisplayName:'LastLoginTime'
            }
        ]
        vm.edit = function() {
            console.log("Edit");
        }
        vm.delete = function() {
            console.log("Ok");
            vm.reloadTable();
        }

        vm.objFuncs = [
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.edit(' + data.Id + ')" ><i class="fa fa-check"></i></a>';
                },
            },
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-danger btn-xs" ng-click="$parent.vm.delete(' + data.Id + ')" ><i class="fa fa-times"></i></a>';
                },
            }
        ]

        vm.reloadTable = function(){
            vm.instance.reloadData(function(data){
                console.log(data);
            },false);
        }

        ////////////////

        function activate() {

        }
        
        activate();
    }
})();