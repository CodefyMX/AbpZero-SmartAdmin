(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.roles.index', RolesController);

    RolesController.$inject = ['$uibModal', 'WebConst', 'abp.services.app.roles'];;
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
        vm.objFuncs = [
            {
                dom: function (data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.createEdit(' + data.Id + ')" ><i class="fa fa-edit"></i></a>';
                },
            },
        ]

        function activate() { }
    }
})();