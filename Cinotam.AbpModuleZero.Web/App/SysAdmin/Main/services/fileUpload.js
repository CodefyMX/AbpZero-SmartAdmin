(function () {
    'use strict';

    angular
        .module('app.web')
        .service('fileUpload', Service);

    Service.$inject = ['$http'];
    function Service($http) {
        this.uploadFile = uploadFile;

        ////////////////

        function uploadFile(file, url, done) {
            var fd = new FormData();
            fd.append('file',file);
            $http.post(url, fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            }).success(function (response) {
                done(null, response);
            }).error(function (e) {
                done(e, null);
            });
        }
    }
})();