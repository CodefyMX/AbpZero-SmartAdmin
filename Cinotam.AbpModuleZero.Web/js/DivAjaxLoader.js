
var divAjaxLoader = (function () {

    var selectors = $("[data-ajax-load]");

    selectors.each(function (i, element) {

        var $element = $(element);

        var url = $element.data("url");

        var parameters = $element.data("parameters");

        var obj = evaluateObj(parameters);

        $element.load(url, obj,function() {
            
        });
    });


    function evaluateObj(json) {
        var nObj = {};
        eval("nObj =" + JSON.parse(JSON.stringify(json)));
        return nObj;

    }

});