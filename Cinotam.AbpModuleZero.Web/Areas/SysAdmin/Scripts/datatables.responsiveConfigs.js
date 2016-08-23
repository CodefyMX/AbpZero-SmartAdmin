
var responsiveHelper_dt_basic = undefined;
var responsiveHelper_dt_roles = undefined;
var responsiveHelper_dt_languages = undefined;
var breakpointDefinition = {
    tablet: 1024,
    phone: 480
};
var options = {
    loadingFunc : loading,
    loadEndFunc: notLoading,
    onErrorFunction:error
};

function loading(button) {
    console.log ("Setting busy");
    abp.ui.setBusy ($("#content"));
}
function notLoading (button) {
    abp.ui.clearBusy($("#content"));
}
function error (message) {
    abp.message.error (message,"Error");
}

window.modalInstance = new abp.app.bootstrap.modal(null,options);