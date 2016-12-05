(function () {
    var controllerId = 'app.views.layout';
    angular.module('app').controller(controllerId,LayoutController);
    LayoutController.$inject = ['$scope'];
    function LayoutController($scope){
    	var vm = this;
    }
})();