(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.roles.index', RolesController);

    RolesController.$inject = ['$uibModal', 'WebConst', 'abp.services.app.role'];;
    function RolesController($uibModal, WebConst, _rolesService) {
        var vm = this;


        activate();
        vm.createEdit = function (id) {
            if (!id) id == null;
            var modalInstance = $uibModal.open({
                templateUrl: WebConst.contentFolder + "roles/createEdit.cshtml",
                controller: 'app.views.roles.createEdit as vm',
                resolve: {
                    items: function () {
                        return {
                            id: id
                        }
                    }
                }
            });
            modalInstance.result.then(function (response) {
                if (response == 'roleEdited') {
                    abp.notify.success(App.localize("RoleEdited"), App.localize("Success"));
                    vm.reloadTable();
                }
            });
        }
        vm.delete = function (id,roleName) {
            var message = abp.utils.formatString(App.localize("RoleDeleteMessage"), roleName);
            abp.message.confirm(message, App.localize("ConfirmQuestion"), function (response) {
                if (response) {
                    _rolesService.deleteRole(id).then(function(){
                        abp.notify.warn(App.localize("RoleDeleted"), App.localize("Success"));
                        vm.reloadTable();
                    });
                }
            });
        }


        ////////////////
        vm.instance = {};
         vm.serverSide = true;
        vm.defaultSearchPropery = 'DisplayName';
        vm.properties = [
            {
                Key: "Id",
                DisplayName: "Id",
                Hidden: true,
            },
            {
                Key: "DisplayName",
                DisplayName: App.localize("Name")
            },
            {
                Key: "CreationTimeString",
                DisplayName: App.localize("CreationTime")
            },
        ];
        vm.url = '/AngularApi/Roles/LoadRoles';
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
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.delete(' + data.Id + ',&quot ' + data.DisplayName + ' &quot)" ><i class="fa fa-trash"></i></a>';
                },
            },
        ]
        vm.reloadTable = function () {
            vm.instance.reloadData(function (data) {
            }, false);
        }
        function activate() { }
    }
})();