(function () {
    if (!window.modalInstance) {
        var options = {
            loadingFunc: loading,
            loadEndFunc: notLoading,
            onErrorFunction: error
        };

        var $content = $("#content");

        function loading(button) {
            console.log("Setting busy");
            abp.ui.setBusy($content);
        }
        function notLoading(button) {
            abp.ui.clearBusy($content);
        }
        function error(message) {
            abp.message.error(message, "Error");
        }

        window.modalInstance = new abp.app.bootstrap.modal(null, options);
    }


})();