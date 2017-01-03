(function () {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.myprofile.index', MyProfileController);

    MyProfileController.$inject = ['fileUpload', 'appSession', '$scope', '$sce', 'abp.services.app.user'];
    function MyProfileController(fileUpload, appSession, $scope, $sce, _userService) {
        var vm = this;
        vm.fileUrl = $sce.trustAsHtml('/Content/Images/placeholder.svg');
        vm.uploadImage = function () {
            fileUpload.uploadFile($scope.image, '/AngularApi/MyProfile/ChangeProfilePicture/' + appSession.user.id, function (error, result) {
                if (result) {
                    vm.fileUrl = $sce.trustAsHtml(result);
                }

            });
        }
        activate();
        vm.userProfile = {};
        ////////////////
        vm.hasPhoneNumber = function (number) {
            console.log('Number', number);
            if (number == null || undefined || '') {
                return false;
            }
            return true;
        }
        function activate() {
            _userService.getUserProfile(appSession.user.id).then(function (response) {
                vm.userProfile = response.data;
                if (vm.userProfile.profilePicture) {
                    vm.fileUrl = $sce.trustAsHtml(vm.userProfile.profilePicture);
                }
            });
        }
    }
})();