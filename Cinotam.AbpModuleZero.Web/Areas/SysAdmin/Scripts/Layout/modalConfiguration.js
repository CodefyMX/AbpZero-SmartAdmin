(function () {
    $(document).ready(function () {
        pageSetUp();
        drawBreadCrumb();

        function loading(button) {
            console.log("Setting busy");
            abp.ui.setBusy($("#content"));
        }
        function notLoading(button) {
            abp.ui.clearBusy($("#content"));
        }
        function error(message) {
            abp.message.error(message, "Error");
        }
        var options = {
            loadingFunc: loading,
            loadEndFunc: notLoading,
            onErrorFunction: error
        };
        window.modalInstance = new abp.app.bootstrap.modal(null, options);

        

    });
})();