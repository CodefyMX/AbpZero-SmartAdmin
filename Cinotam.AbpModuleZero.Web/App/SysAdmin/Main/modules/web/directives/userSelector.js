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
    UserSelectorController.$inject = ['abp.services.app.user','DTOptionsBuilder','DTColumnBuilder','$compile','$scope'];
    function UserSelectorController (_userService,DTOptionsBuilder,DTColumnBuilder,$compile,$scope) {
        var vm = this;
        vm.users = [];

        vm.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax',{
            url:'/AngularApi/Users/LoadUsers',
            type:'GET'
        }).withDataProp('data').withOption('processing', true)
        .withOption('serverSide', true).withPaginationType('full_numbers').withOption('createdRow', createdRow);
        vm.dtColumns = [
            DTColumnBuilder.newColumn('Id').withTitle('ID'),
            DTColumnBuilder.newColumn('UserName').withTitle('First name'),
            DTColumnBuilder.newColumn(null).withTitle(App.localize('Actions')).notSortable()
            .renderWith(actions)
        ];
        vm.onClickFunction = function(id){
            console.log(id);
            ctrlFun(id);
        }

        function actions(data,type,full,meta){
            return '<a class="btn btn-default btn-xs" ng-click="vm.onClickFunction(' + data.Id + ')" ><i class="fa fa-check"></i></a>'
            
        }
        function createdRow(row, data, dataIndex) {
            // Recompiling so we can bind Angular directive to the DT
            $compile(angular.element(row).contents())($scope);
        }

    }
})();