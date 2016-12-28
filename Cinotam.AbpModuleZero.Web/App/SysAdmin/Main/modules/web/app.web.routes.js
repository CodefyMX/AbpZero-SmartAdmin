(function () {
	'use strict';

	angular
		.module('app.web').run(appRun);
	/* @ngInject */
	appRun.$inject = ['routerHelper'];
	function appRun(routerHelper) {
		var states = getStates(routerHelper);
		routerHelper.configureStates(states, '/dashboard');
	}
	function getStates(routerHelper) {
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
		//Creates the child route for org users
		var usersForOrganizationUnitsMenu = new routerHelper.createSimpleMenuItem('OrganizationUnits.Orgunitusers', {
			templateUrl: webFolder + 'organizationunits/orgunitsusers.cshtml',
			url: '/orgusers/:id'
		});
		var languageTexts = new routerHelper.createSimpleMenuItem('LanguageTexts', {
			templateUrl: webFolder + 'languages/languageTexts/index.cshtml',
			url: '/EditLanguageTexts/:targetLang'
		});
		//Registers the route
		routeObj.routes.push(usersForOrganizationUnitsMenu);
		routeObj.routes.push(languageTexts);
		return routeObj;
	}
})();
