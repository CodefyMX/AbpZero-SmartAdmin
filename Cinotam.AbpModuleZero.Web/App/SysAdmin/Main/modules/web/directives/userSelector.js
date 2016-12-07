(function() {
    'use strict';
    var ctrlFun;
    angular
        .module('app.web')
        .directive('userSelector', UserSelector);
    var passedFunction;
    UserSelector.$inject = [];
    function UserSelector() {
        var webFolder = '/App/SysAdmin/Main/modules/web/';
        // Usage:
        //  <user-selector user-selected='$scope.onSelected()' />
        // Creates:
        //  Table of users
        
        var directive = {
            bindToController: true,
            controller: UserSelectorController,
            controllerAs: 'vm',
            link: link,
            restrict: 'E',
            templateUrl:webFolder+'directives/UserSelector.cshtml',
            scope: {
                fromCtrl:'=onselected',
            }
        };
        return directive;
        
        function link(scope, element, attrs) {
            if(scope.vm.fromCtrl){

                ctrlFun = scope.vm.fromCtrl;
            }else{
                ctrlFun = function(){ console.warn("No user selected event function defined"); }
            }
        }
    }
    /* @ngInject */
    UserSelectorController.$inject = ['$scope','abp.services.app.user'];
    function UserSelectorController ($scope,_userService) {
        var vm = this;
        vm.users = [];
        _userService.getUsers().then(function(response){
            console.log(response.data);
            vm.users = response.data.items
        });
        vm.onClickFunction = function(id){
            ctrlFun(id);
        }
    }
})();