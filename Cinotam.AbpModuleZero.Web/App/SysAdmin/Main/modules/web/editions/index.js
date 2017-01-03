(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.editions.index', EditionsController);

    EditionsController.$inject = ['$uibModal', 'abp.services.app.featureService', 'WebConst'];
    function EditionsController($uibModal, _featureService, WebConst) {
        var vm = this;

        vm.editions = [];
        function activate() {
            _featureService.getEditions().then(function (response) {
                vm.editions = response.data.editions;
            });
        }
        vm.createEditEdition = function (id) {
            var modalInstance = $uibModal.open({
                templateUrl: WebConst.contentFolder + 'editions/CreateEdit.cshtml',
                controller: 'app.views.editions.createEdit as vm',
                resolve: {
                    items: function () {
                        return {
                            id: id
                        }
                    }
                }
            });
        }
        vm.removeEdition = function (id) {

        }
        activate();




    }
})();