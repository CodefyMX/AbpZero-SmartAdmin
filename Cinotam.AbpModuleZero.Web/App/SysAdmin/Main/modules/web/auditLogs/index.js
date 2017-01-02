(function() {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.auditLogs.index', AuditLogsController);

    AuditLogsController.$inject = ['$uibModal','WebConst'];
    function AuditLogsController($uibModal,WebConst) {
        var vm = this;
        ////////////////


        vm.instance = {};
        vm.serverSide = true;
        vm.defaultSearchPropery = 'Name';
        vm.properties = [
            {
                Key: "MethodName",
                DisplayName: App.localize("MethodName")

            },
            {
                Key: "ServiceName",
                DisplayName: App.localize("ServiceName"),

            },
            {
                Key: "UserName",
                DisplayName: App.localize('UserName'),
            },
            {
                Key: "ClientIpAddress",
                DisplayName: App.localize('IP'),
                Responsive: true
            },
            {
                Key: "BrowserInfo",
                DisplayName: App.localize('BrowserInfo'),
            }
        ];
        vm.objFuncs = [
            {
                dom: function(data, type, full, meta) {
                    //$parent.vm.click refers to this controller
                    return '<a class="btn btn-default btn-xs" ng-click="$parent.vm.openModal(' + data.Id + ')" ><i class="fa fa-edit"></i></a>';
                }
            }
        ];
        vm.url = '/AngularApi/AuditLogs/LoadLogs';

        vm.reloadTable = function() {
            vm.instance.reloadData(function(data) {
            }, false);
        }

        vm.openModal = function(id) {
            var modalInstance = $uibModal.open({
                templateUrl: WebConst.contentFolder + 'dashboard/logDetails.cshtml',
                controller: 'app.views.dashboard.logDetails as vm',
                resolve: {
                    items: function() {
                        return id;
                    }
                }
            });
        }

        function activate() {
        }

        activate();
    }
})();