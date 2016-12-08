(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.organizationUnits.addUser', AddUserController);

    AddUserController.$inject = ['items', '$uibModalInstance', 'abp.services.app.organizationUnits', '$sce'];
    function AddUserController(items, uibModalInstance, _orgUnits, $sce) {
        var vm = this;
        vm.cancel = function () {
            uibModalInstance.close();
        }
        vm.userId = 0;
        vm.click = function (id) {
            vm.userId = id;

            _orgUnits.addUserToOrgUnit({
                UserId: vm.userId,
                OrgUnitId: items.orgUnitId
            }).then(function () {
                uibModalInstance.close("useradded");
            });
        }
        vm.delete = function (id) {
            vm.userId = id;

            alert(id);
        }
        vm.properties = [
            {
                Key: "Id",
                DisplayName: "Id",
                Hidden: true
            },
            {
                Key: "UserName",
                DisplayName: "UserName"
            },
            {
                Key: "EmailAddress",
                DisplayName: "Email"
            }
        ]

        vm.objFuncs = [
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.click(' + data.Id + ')" ><i class="fa fa-check"></i></a>';
                },
            }
        ]

        vm.actions = function (data, type, full, meta) {
            return '<a class="btn btn-default btn-xs" ng-click="vm.onClickFunction(' + data.Id + ')" ><i class="fa fa-check"></i></a>'
        }
        activate();
        ////////////////

        function activate() { }
    }
})();