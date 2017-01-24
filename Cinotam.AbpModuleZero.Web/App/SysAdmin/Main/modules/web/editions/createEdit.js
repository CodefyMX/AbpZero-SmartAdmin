(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.editions.createEdit', CreateEditController);

    CreateEditController.$inject = ['$uibModalInstance', 'items', 'abp.services.app.featureService'];
    function CreateEditController($uibModalInstance, items, _featureService) {
        var vm = this;


        activate();
        vm.cancel = function () {
            $uibModalInstance.close();
        }
        vm.submit = function () {

        }
        vm.edition = {};
        ////////////////

        function activate() {
            _featureService.getEditionForEdit(items.id).then(function (response) {
                vm.edition = response.data;
                buildFeaturesTree(vm.edition.features);

            });
        }
        function buildFeaturesTree(features){
            console.log(features);
        }
    }
})();