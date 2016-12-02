(function() {
    var controllerId = 'app.views.home';
    angular.module('app').controller(controllerId,HomeController);

    HomeController.$inject = ['$scope'];
    function HomeController($scope){
    	var vm = this;
    }
})();