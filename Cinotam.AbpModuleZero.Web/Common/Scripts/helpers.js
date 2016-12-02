var App = App || {};
(function () {

    var appLocalizationSource = abp.localization.getSource('AngularApp');
    App.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };

})(App);