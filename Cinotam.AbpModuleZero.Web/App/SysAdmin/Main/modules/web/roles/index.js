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

                }
            });
        }



        ////////////////
        vm.instance = {};
        vm.defaultSearchPropery = 'UserName';
        vm.properties = [
            {
                Key: "Id",
                DisplayName: "Id",
                Hidden: true,
            },
            {
                Key: "DisplayName",
                DisplayName: "Name"
            },
            {
                Key: "CreationTimeString",
                DisplayName: "Creation Time"
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
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.delete(' + data.Id + ')" ><i class="fa fa-trash"></i></a>';
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