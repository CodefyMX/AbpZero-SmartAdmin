(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.users.changeRoles', ChangeRolesController);

    ChangeRolesController.$inject = ['$uibModalInstance', 'items', 'abp.services.app.user'];
    function ChangeRolesController($uibModalInstance, items, _userService) {
        var vm = this;
        vm.cancel = function () {
            $uibModalInstance.close();
        }
        vm.submit = function (form) {
           var postRequest = {
               userId:vm.rolesModel.userId,
               roles:[]
           }
           vm.rolesModel.roles.forEach(function(role){
               if(role.isSelected){
                   postRequest.roles.push(role.name);
               }
           });
           _userService.setUserRoles(postRequest).then(function(){
               $uibModalInstance.close("roleschanged");
           });
        }
        vm.rolesModel = {
            userId:0,
            userName:'',
            roles:[]
        };

        activate();

        ////////////////

        function activate() {

            _userService.getRolesForUser(items.userId).then(function (response) {
                var data = response.data;
                vm.rolesModel.userId = items.userId;
                vm.rolesModel.userName = data.fullName;
                vm.rolesModel.roles = data.roleDtos;
            })
        }
    }
})();