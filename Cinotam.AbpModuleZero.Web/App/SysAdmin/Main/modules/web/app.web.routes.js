(function () {
	'use strict';

	angular
	.module('app.web').run(appRun);
	/* @ngInject */
	appRun.$inject = ['routerHelper'];
	function appRun(routerHelper) {
		routerHelper.configureStates(getStates(routerHelper), '/');
	}
	function getStates(routerHelper) {
		var routeObj = {
		    name: abp.nav.menus.ModuleZeroMenu.name,
				routes: []
		};
		abp.nav.menus.ModuleZeroMenu.items[0].items.forEach(function (menuItem) {
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
		return routeObj;
	}

})();
