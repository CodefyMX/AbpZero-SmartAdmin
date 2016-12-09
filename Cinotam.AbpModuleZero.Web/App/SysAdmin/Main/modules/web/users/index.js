(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.users.index', IndexController);
    IndexController.$inject = ['$uibModal', 'WebConst'];
    function IndexController($uibModal, webConst) {
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
                Key: 'CreationTimeString',
                DisplayName: 'CreationTime'
            },
            {
                Key: 'LastLoginTimeString',
                DisplayName: 'LastLoginTime'
            }
        ]
        vm.createEdit = function (id) {
            var modalInstance = $uibModal.open({
                templateUrl: webConst.contentFolder + 'users/createedit.cshtml',
                controller: 'app.views.users.createEdit as vm',
                resolve: {
                    items: function () {
                        if (id) {
                            return {
                                userId:id
                            };
                        }
                    }
                }
            });
            modalInstance.result.then(function (response) {
                console.log(response);
            });
        }
        vm.delete = function () {
            console.log("Ok");
            vm.reloadTable();
        }

        vm.objFuncs = [
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.createEdit(' + data.Id + ')" ><i class="fa fa-edit"></i></a>';
                },
            },
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.changeRoles(' + data.Id + ')" ><i class="fa fa-briefcase"></i></a>';
                },
            },
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.changePassword(' + data.Id + ')" ><i class="fa fa-key"></i></a>';
                },
            },
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.unlock(' + data.Id + ')" ><i class="fa fa-unlock"></i></a>';
                },
            },
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.changePermissions(' + data.Id + ')" ><i class="fa fa-lock"></i></a>';
                },
            },
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.delete(' + data.Id + ')" ><i class="fa fa-trash"></i></a>';
                },
            }
        ]

        vm.reloadTable = function () {
            vm.instance.reloadData(function (data) {
                console.log(data);
            }, false);
        }

        ////////////////

        function activate() {

        }

        activate();
    }
})();