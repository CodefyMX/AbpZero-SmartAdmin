﻿var App = App || {};
(function () {

    var appLocalizationSource = abp.localization.getSource('AbpModuleZero');
    App.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };

})(App);