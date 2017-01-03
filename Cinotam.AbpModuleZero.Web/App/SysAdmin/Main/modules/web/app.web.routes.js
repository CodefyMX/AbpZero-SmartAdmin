(function () {
	'use strict';

	angular
		.module('app.web').run(appRun);
	/* @ngInject */
	appRun.$inject = ['routerHelper', 'appSession'];
	function appRun(routerHelper, appSession) {
		var states = getStates(routerHelper, appSession);
		routerHelper.configureStates(states, '/dashboard');
	}
	function getStates(routerHelper, appSession) {
		var webFolder = '/App/SysAdmin/Main/modules/web/';
		var moduleZeroMenu = abp.nav.menus.ModuleZeroMenu;
		var routeObj = {
			name: moduleZeroMenu.name,
			routes: []
		};
		moduleZeroMenu.items[0].items.forEach(function (menuItem) {
			if (menuItem.customData.angularMenu) {
				var angularCustomData = menuItem.customData.angularMenu;
				if (angularCustomData.HasPermission) {
					if (abp.auth.hasPermission(angularCustomData.PermissionName)) {
						routeObj.routes.push(new routerHelper.createMenuItem(menuItem));
					}
				} else {
					var result = new routerHelper.createMenuItem(menuItem);
					routeObj.routes.push(result);
				}
			}
		});
		if (appSession.isLoggedIn) {
			//Creates the child route for org users
			var usersForOrganizationUnitsMenu = new routerHelper.createSimpleMenuItem('OrganizationUnits.Orgunitusers', {
				templateUrl: webFolder + 'organizationunits/orgunitsusers.cshtml',
				url: '/orgusers/:id'
			});
			var languageTexts = new routerHelper.createSimpleMenuItem('LanguageTexts', {
				templateUrl: webFolder + 'languages/languageTexts/index.cshtml',
				url: '/EditLanguageTexts/:targetLang'
			});
			var myProfile = new routerHelper.createSimpleMenuItem('MyProfile', {
				templateUrl: webFolder + 'myprofile/index.cshtml',
				url: '/MyProfile'
			});
			//Registers the route
			routeObj.routes.push(usersForOrganizationUnitsMenu);
			routeObj.routes.push(languageTexts);
			routeObj.routes.push(myProfile);
		}
		return routeObj;
	}
})();
