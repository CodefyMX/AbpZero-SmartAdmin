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
        'WebConst',
        'abp.services.app.settings'
    ];
    function MyProfileController(fileUpload, appSession, $scope, $sce, _userService, $uibModal, WebConst, _settingsService) {
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
        vm.toggleTwoFactor = function() {
            _userService.enableOrDisableTwoFactorAuthForUser(vm.userProfile.id).then(function() {
                activate();
            });
        }
        function activate() {
            _userService.getUserProfile(appSession.user.id).then(function(response) {
                vm.userProfile = response.data;
                if (vm.userProfile.profilePicture) {
                    vm.fileUrl = $sce.trustAsHtml(vm.userProfile.profilePicture);
                }
            });
            configureNotifications();
            getUserNotifications();
        }

        function checkNotificationStatus() {
            vm.notifications.forEach(function(element) {
                _settingsService.isSubscribed(element.name).then(function(response) {
                    if (response.data) {
                        element.isSubscribed = true;
                    }
                });
            });
        }
        vm.notifications = [];
        vm.userNotifications = [];
        vm.subscribe = function(notification) {
            abp.ui.setBusy();
            if (notification.isSubscribed) {
                _settingsService.subscribeToNotification(notification.name).then(function() {
                    abp.ui.clearBusy();
                });
            }
            else {
                _settingsService.unSubscribeToNotification(notification.name).then(function() {
                    abp.ui.clearBusy();
                });
            }
        }
        vm.markAsReaded = function(notification) {
            _userService.markAsReaded(notification.id).then(function() {
                notification.readed = true;
            });
        }
        activate();
        function configureNotifications() {
            if (abp.auth.isGranted('Pages.SysAdmin.Users')) {
                vm.notifications.push({
                    name: 'UserDeleted',
                    displayName: App.localize("SubscribeToNotificationUserCreated"),
                    isSubscribed: false
                });
                vm.notifications.push({
                    name: 'UserCreated',
                    displayName: App.localize("SubscribeToNotificationUserDeleted"),
                    isSubscribed: false
                });
            }
            if (abp.auth.isGranted('Pages.SysAdminRoles')) {
                vm.notifications.push({
                    name: 'RoleCreated',
                    displayName: App.localize("SubscribeToNotificationRoleCreated"),
                    isSubscribed: false
                });
                vm.notifications.push({
                    name: 'RoleDeleted',
                    displayName: App.localize("SubscribeToNotificationRoleDeleted"),
                    isSubscribed: false
                });
                vm.notifications.push({
                    name: 'RoleEdited',
                    displayName: App.localize("SubscribeToNotificationRoleEdited"),
                    isSubscribed: false
                });
            }
            if (abp.auth.isGranted('Pages.SysAdminLanguages')) {
                vm.notifications.push({
                    name: 'LanguageCreated',
                    displayName: App.localize("SubscribeToNotificationLanguageCreated"),
                    isSubscribed: false
                });
                vm.notifications.push({
                    name: 'LanguageDeleted',
                    displayName: App.localize("SubscribeToNotificationLanguageDeleted"),
                    isSubscribed: false
                });
            }
            if (abp.auth.isGranted('Pages.SysAdminConfiguration')) {
                vm.notifications.push({
                    name: 'SettingsChanged',
                    displayName: App.localize("SubscribeToNotificationSettingsChanged"),
                    isSubscribed: false
                });
            }
            if (abp.auth.isGranted('Pages.Tenants')) {
                vm.notifications.push({
                    name: 'TenantCreated',
                    displayName: App.localize("SubscribeToNotificationsTenantCreated"),
                    isSubscribed: false
                });
                vm.notifications.push({
                    name: 'TenantDeleted',
                    displayName: App.localize("SubscribeToNotificationsTenantDeleted"),
                    isSubscribed: false
                });
            }

            checkNotificationStatus();
        }
        function getUserNotifications() {
            _userService.getMyNotifications(0, 10).then(function(response) {
                response.data.notifications.forEach(function(userNotification) {
                    var message = abp.notifications.getFormattedMessageFromUserNotification(userNotification);
                    vm.userNotifications.push({ message: message, id: userNotification.id, readed: false });
                });
            });
        }
    }
})();