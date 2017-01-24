(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.users.index', IndexController);
    IndexController.$inject = ['$uibModal', 'WebConst', 'abp.services.app.user'];
    function IndexController($uibModal, webConst, _usersService) {
        var vm = this;
        vm.instance = {};
        vm.defaultSearchPropery = 'UserName';
        vm.properties = [
            {
                Key: "Id",
                DisplayName: "Id",
                Hidden: true,
            },
            {
                Key: "UserName",
                DisplayName: App.localize("UserName")
            },
            {
                Key: "Name",
                DisplayName: App.localize("Name")
            },
            {
                Key: "EmailAddress",
                DisplayName: App.localize("EmailAddress")
            },
            {
                Key: 'CreationTimeString',
                DisplayName: App.localize("CreationTime")
            },
            {
                Key: 'LastLoginTimeString',
                DisplayName: App.localize("LastLoginTime")
            }
        ];
        vm.url = '/AngularApi/Users/LoadUsers';
        vm.createEdit = function (id) {
            var modalInstance = $uibModal.open({
                templateUrl: webConst.contentFolder + 'users/createedit.cshtml',
                controller: 'app.views.users.createEdit as vm',
                resolve: {
                    items: function () {
                        if (id) {
                            return {
                                userId: id
                            };
                        }
                        else {
                            return {};
                        }
                    }
                }
            });
            modalInstance.result.then(function (response) {
                if (response === "userCreated") {
                    abp.notify.success(App.localize("UserCreated"), App.localize("Success"));
                    vm.reloadTable();
                }
            });
        }
        vm.delete = function () {
            vm.reloadTable();
        }
        vm.unlock = function (id) {
            abp.ui.setBusy();
            _usersService.unlockUser(id).then(function () {
                abp.notify.success(App.localize("UserUnlocked"), App.localize("Success"));
                abp.ui.clearBusy();
            });
        }
        vm.delete = function (id, userName) {

            var confirmDelete = abp.utils.formatString(App.localize("ConfirmDeleteUser"), userName);
            abp.message.confirm(confirmDelete,
                App.localize("ConfirmQuestion"),
                function (response) {
                    if (response) {
                        abp.ui.setBusy();
                        _usersService.deleteUser(id)
                            .then(function () {
                                abp.notify.warn(App.localize("UserDeleted"), App.localize("Success"));
                                abp.ui.clearBusy();
                                vm.reloadTable();
                            });
                    }
                });


        }
        vm.changeRoles = function (userId) {
            var modalInstance = $uibModal.open({
                templateUrl: webConst.contentFolder + "users/changeRoles.cshtml",
                controller: "app.views.users.changeRoles as vm",
                resolve: {
                    items: function () {
                        return {
                            userId: userId
                        }
                    }
                }
            });
            modalInstance.result.then(function (response) {
                if (response === "roleschanged") {
                    abp.notify.success(App.localize("RolesChanged"), App.localize("Success"));
                }

            });
        }
        vm.changePassword = function (userId, userName) {
            var modalInstance = $uibModal.open({
                templateUrl: webConst.contentFolder + "users/changePassword.cshtml",
                controller: "app.views.users.changePassword as vm",
                resolve: {
                    items: function () {
                        return {
                            userId: userId,
                            userName: userName
                        }
                    }
                }
            });
            modalInstance.result.then(function (response) {
                if (response === "passwordchanged") {
                    abp.notify.success(App.localize("PasswordChanged"), App.localize("Success"));
                }

            });
        }
        vm.changePermissions = function (userId, userName) {
            var modalInstance = $uibModal.open({
                templateUrl: webConst.contentFolder + "users/changePermissions.cshtml",
                controller: "app.views.users.changePermissions as vm",
                resolve: {
                    items: function () {
                        return {
                            userId: userId,
                            userName: userName
                        }
                    }
                }
            });
            modalInstance.result.then(function (response) {
                if (response === "permissionsset") {
                    abp.notify.success(App.localize("PermissionsSet"), App.localize("Success"));
                }
            });
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
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.changePassword(' + data.Id + ',&quot ' + data.UserName + ' &quot)" ><i class="fa fa-key"></i></a>';
                },
            },
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.changePermissions(' + data.Id + ',&quot ' + data.UserName + ' &quot)" ><i class="fa fa-lock"></i></a>';
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
                    //
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.delete(' + data.Id + ',&quot ' + data.UserName + ' &quot)" ><i class="fa fa-trash"></i></a>';
                },
            }
        ]
        vm.serverSide = true;
        vm.reloadTable = function () {
            vm.instance.reloadData(function (data) {
            }, false);
        }

        ////////////////

        function activate() {

        }

        activate();
    }
})();