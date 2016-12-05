(function () {
    angular.module('app').factory('appSession', [
            function () {
                var _session = {
                    user: null,
                    tenant: null,
                    isLoggedIn: false,
                    checkLoginInfo: checkLoginInfo
                };
                function checkLoginInfo() {
                    abp.services.app.session.getCurrentLoginInformations({ async: false }).done(function (result) {
                       _session.user = result.user;
                        _session.tenant = result.tenant;
                        _session.isLoggedIn = true;

                    });
                }


                checkLoginInfo();


                return _session;
            }
    ]);
})();