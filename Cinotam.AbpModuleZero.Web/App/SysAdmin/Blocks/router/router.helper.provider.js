/* Help configure the state-base ui.router */
(function () {
    'use strict';

    angular
    .module('blocks.router')
    .provider('routerHelper', routerHelperProvider);

    routerHelperProvider.$inject = ['$locationProvider', '$stateProvider', '$urlRouterProvider'];
    /* @ngInject */
    function routerHelperProvider($locationProvider, $stateProvider, $urlRouterProvider) {

        /* jshint validthis:true */
        var config = {
            docTitle: undefined,
            resolveAlways: {}
        };

        if (!(window.history && window.history.pushState)) {
            window.location.hash = '/';
        }

        $locationProvider.html5Mode(false);

        this.configure = function (cfg) {
            angular.extend(config, cfg);
        };

        this.$get = RouterHelper;
        RouterHelper.$inject = ['$location', '$rootScope', '$state', 'logger'];
        /* @ngInject */
        function RouterHelper($location, $rootScope, $state, logger) {
            var handlingStateChangeError = false;
            var hasOtherwise = false;
            var stateCounts = {
                errors: 0,
                changes: 0
            };

            var service = {
                configureStates: configureStates,
                getStates: getStates,
                stateCounts: stateCounts,
                createMenuItem: createMenuItem
            };

            //Abp extension
            var abpRoutes = [];
            init();

            return service;

            ///////////////
            function createMenuItem(menuItem) {
                var self = this;
                var config = {};
                config.displayName = menuItem.displayName;
                config.url = menuItem.customData.angularMenu.uiUrl;
                config.order = menuItem.order;
                config.name = menuItem.name;
                config.icon = menuItem.icon;
                config.templateUrl = menuItem.customData.angularMenu.templateUrl;
                self.state = menuItem.customData.angularMenu.stateName;
                self.config = config;
                self.items = [];
                self.isOtherApp = menuItem.customData.angularMenu.isOtherApp;
                if (menuItem.items.length > 0) {
                    menuItem.items.forEach(function (childItem) {
                        console.log("Childs",childItem);
                        self.items.push(childItem);
                    });
                }
                return self;
            }
            function configureStates(routeObj, otherwisePath) {
                var routes = [];
                routeObj.routes.forEach(function (data) {
                   
                        var state = data;
                        // if (!state.isOtherApp) {
                        //     state.config.resolve =
                        //     angular.extend(state.config.resolve || {}, config.resolveAlways);
                        //     $stateProvider.state(state.state, state.config);
                        // }
                        routes.push({
                            name: state.state,
                            config: state.config,
                            isOtherApp: state.isOtherApp
                        });
                    
                });
                if (otherwisePath && !hasOtherwise) {
                    hasOtherwise = true;
                    $urlRouterProvider.otherwise(otherwisePath);
                }
                abpRoutes.push({
                    menuName: routeObj.name,
                    items: routes
                });
                console.log("Abp routes",abpRoutes);
            }

            function handleRoutingErrors() {
                // Route cancellation:
                // On routing error, go to the dashboard.
                // Provide an exit clause if it tries to do it twice.
                $rootScope.$on('$stateChangeError',
                function (event, toState, toParams, fromState, fromParams, error) {
                    if (handlingStateChangeError) {
                        return;
                    }
                    stateCounts.errors++;
                    handlingStateChangeError = true;
                    var destination = (toState &&
                      (toState.title || toState.name || toState.loadedTemplateUrl)) ||
                      'unknown target';
                    var msg = 'Error routing to ' + destination + '. ' +
                    (error.data || '') + '. <br/>' + (error.statusText || '') +
                    ': ' + (error.status || '');
                    logger.warning(msg, [toState]);
                    $location.path('/');
                }
                );
            }

            function init() {
                handleRoutingErrors();
                updateDocTitle();
            }

            function getStates(menuName) {
                if(menuName == "" || undefined) throw error;
                for (var i = 0; i < abpRoutes.length; i++) {
                    if (abpRoutes[i].menuName == menuName) return abpRoutes[i];
                }
            }

            function updateDocTitle() {
                $rootScope.$on('$stateChangeSuccess',
                function (event, toState, toParams, fromState, fromParams) {
                    stateCounts.changes++;
                    handlingStateChangeError = false;
                    var title = config.docTitle + ' ' + (toState.title || '');
                    $rootScope.title = title; // data bind to <title>
                }
              );
            }
        }
    }
})();
