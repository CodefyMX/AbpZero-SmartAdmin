(function () {
    var controllerId = 'app.views.layout.header';
    angular.module('app').controller(controllerId, HeaderController);
    HeaderController.$inject = ['$rootScope', '$state', 'appSession', 'routerHelper'];
    function HeaderController($rootScope, $state, appSession, routerHelper) {
        var vm = this;

        vm.languages = abp.localization.languages;
        vm.currentLanguage = abp.localization.currentLanguage;

        vm.currentMenuName = $state.current.menu;
        var routes = routerHelper.getStates("ModuleZeroMenu");
        vm.menu = routes;
        $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            vm.currentMenuName = toState.menu;
        });
        vm.isLoggedIn = appSession.isLoggedIn;
        vm.getShownUserName = function () {
            if (!abp.multiTenancy.isEnabled) {
                return appSession.user.userName;
            } else {
                if (appSession.tenant) {
                    return appSession.tenant.tenancyName + '\\' + appSession.user.userName;
                } else {
                    return '.\\' + appSession.user.userName;
                }
            }
        };
        abp.event.on('abp.notifications.received', function (userNotification) {
            if (userNotification.notification.data.type === "Cinotam.ModuleZero.Notifications.Chat.Outputs.ChatData") {
                sendChatNotification(userNotification);
            } else {
                abp.notifications.showUiNotifyForUserNotification(userNotification);
            }
        });
    }

})();
