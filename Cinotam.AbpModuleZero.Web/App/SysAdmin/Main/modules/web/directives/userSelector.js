(function() {
    'use strict';
    angular
        .module('app.web')
        .directive('userSelector', UserSelector);
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
            templateUrl: webFolder + 'directives/UserSelector.cshtml',
            scope: {
                tProperties: '=properties',
                tActions: '=actions',
                tFunctions: '=funcobj'
            }
        };
        return directive;
    }
    function link(scope, element, attrs) {
    }
    /* @ngInject */
    UserSelectorController.$inject = ['abp.services.app.user', 'DTOptionsBuilder', 'DTColumnBuilder', '$compile', '$scope'];
    function UserSelectorController(_userService, DTOptionsBuilder, DTColumnBuilder, $compile, $scope) {
        var vm = this;
        //Holds the data table instance in the vm.instance variable of the parent
        vm.dtInstance = function(instance) {
            $scope.$parent.vm.instance = instance;
        };
        vm.users = [];
        vm.dtColumns = [];
        vm.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
            url: '/AngularApi/Users/LoadUsers',
            type: 'GET'
        }).withDataProp('data')
            .withOption('processing', true).withOption('createdRow', createdRow)
            .withOption('serverSide', true).withOption('createdRow', createdRow)
            .withPaginationType('full_numbers').withOption('createdRow', createdRow);


        vm.dtColumns = buildColumns($scope.vm.tProperties);

        vm.dtColumns.push(DTColumnBuilder.newColumn(null)
            .withTitle(App.localize("Actions"))
            .notSortable()
            .withClass('text-center')
            .renderWith(loadCustom));

        vm.registeredFunctions = [];

        /**
             * Push all the functions inside the registeredFunctions array
             * @param  data, type, full, meta
             */
        function loadCustom(data, type, full, meta) {
            var btns = " ";
            for (var i = 0; i < $scope.vm.tFunctions.length; i++) {
                var current = $scope.vm.tFunctions[i];
                btns += current.dom(data, type, full, meta).toString();
                vm.registeredFunctions.push({
                    name: current.name,
                    func: current.action
                });
            }
            return btns;
        }
        /**
             * Recompiles the rows table to allow angular binding
             * @param  row, data, dataIndex
             */
        function createdRow(row, data, dataIndex) {
            // Recompiling so we can bind Angular directive to the DT
            $compile(angular.element(row).contents())($scope);
        }
        /**
             * Recompiles the table columns to allow angular binding
             * @param  row, data, dataIndex
             */
        function createdColumn(col, data, dataIndex) {
            $compile(angular.element(col).contents())($scope);
        }
        /**
             * Build the columns for the table
             * @param properties the array of properties [Key,DisplayName(localizable)] that the directive should display in the table
             * @returns {DTColumns} the options
             */
        function buildColumns(ctrlProperties) {
            if (!ctrlProperties) ctrlProperties = [];
            var columns = [];
            if (ctrlProperties.length <= 0) {
                ctrlProperties = [
                    {
                        Key: "Id",
                        DisplayName: "Id"
                    },
                    {
                        Key: "UserName",
                        DisplayName: "UserName"
                    }
                ];
            }

            for (var i = 0; i < ctrlProperties.length; i++) {
                var currentProperty = ctrlProperties[i];

                if (currentProperty.Hidden) {
                    columns.push(
                        DTColumnBuilder.newColumn(currentProperty.Key)
                            .withTitle(App.localize(currentProperty.DisplayName)).notVisible().withOption(createdColumn));
                }
                else {
                    columns.push(
                        DTColumnBuilder.newColumn(currentProperty.Key)
                            .withTitle(App.localize(currentProperty.DisplayName)).withOption(createdColumn));
                }

            }
            return columns;
        }

    }
})();