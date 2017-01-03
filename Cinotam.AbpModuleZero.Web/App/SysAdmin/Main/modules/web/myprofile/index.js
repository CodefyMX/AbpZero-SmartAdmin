(function() {
    'use strict';

    angular
        .module('app.web')
        .controller('app.views.myprofile.index', MyProfileController);

    MyProfileController.$inject = [
        'fileUpload',
        'appSession',
        '$scope',
        '$sce',
        'abp.services.app.user',
        '$uibModal',
        'WebConst'
    ];
    function MyProfileController(fileUpload, appSession, $scope, $sce, _userService, $uibModal, WebConst) {
        var vm = this;
        vm.fileUrl = $sce.trustAsHtml('/Content/Images/placeholder.svg');
        vm.uploadImage = function() {
            abp.ui.setBusy();
            fileUpload.uploadFile($scope.image, '/AngularApi/MyProfile/ChangeProfilePicture/' + appSession.user.id, function(error, result) {
                if (result) {
                    abp.ui.clearBusy();
                    vm.fileUrl = $sce.trustAsHtml(result);
                    vm.userProfile.profilePicture = result;
                    abp.message.success(App.localize("ProfilePicModified"), App.localize("Success"));
                }
                else {
                    abp.ui.clearBusy();
                }
            });
        }
        activate();
        vm.userProfile = {};
        ////////////////
        vm.hasPhoneNumber = function(number) {
            
            if (number == null || undefined || '') {
                return false;
            }
            return true;
        }
        vm.changePhoneNumber = function() {
            var modalInstance = $uibModal.open({
                templateUrl: WebConst.contentFolder + "myprofile/addPhoneNumber.cshtml",
                controller: "app.views.myprofile.changePhone as vm"
            });
            modalInstance.result.then(function(response) {
                if (response === 'confirmed') {
                    abp.notify.success(App.localize("PhoneConfirmed"), App.localize("Success"));
                    activate();
                }
            });
        }
        vm.changePassword = function() {
            var userId = vm.userProfile.id;
            var userName = vm.userProfile.userName;
            var modalInstance = $uibModal.open({
                templateUrl: WebConst.contentFolder + "users/changePassword.cshtml",
                controller: "app.views.users.changePassword as vm",
                resolve: {
                    items: function() {
                        return {
                            userId: userId,
                            userName: userName
                        }
                    }
                }
            });
            modalInstance.result.then(function(response) {
                if (response === "passwordchanged") {
                    abp.notify.success(App.localize("PasswordChanged"), App.localize("Success"));
                }
            });
        }
        vm.submit = function() {
            _userService.createUser(vm.userProfile).then(function() {
                abp.message.success(App.localize("ProfileModified"), App.localize("Success"));
            });
        }
        function activate() {
            _userService.getUserProfile(appSession.user.id).then(function(response) {
                vm.userProfile = response.data;
                if (vm.userProfile.profilePicture) {
                    vm.fileUrl = $sce.trustAsHtml(vm.userProfile.profilePicture);
                }
            });
        }
    }
})();