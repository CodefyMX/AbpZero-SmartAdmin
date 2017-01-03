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


                //if you are using spa in the main page better user this

                //getCurrentLoginInformationsSpa 

                // abp.services.app.session.getCurrentLoginInformationsSpa({ async: false }).done(function (result) {

                //     if (result) {
                //         _session.user = result.user;
                //         _session.tenant = result.tenant;
                //         _session.isLoggedIn = true;
                //     }
                //     else {
                //         _session.isLoggedIn = false;
                //     }
                // });

                // if the user is not logged in it will return null as the response

                // the default method will throw an exception and will be handled by abp
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