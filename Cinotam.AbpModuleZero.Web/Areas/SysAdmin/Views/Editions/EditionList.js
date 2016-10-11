(function () {


    $(document).ready(function () {

        var $body = $("body");

        $body.on("click", ".js-delete-edition", function () {
            var id = $(this).data("id");
            deleteEdition(id);
        });


        function deleteEdition(editionId) {
            abp.message.confirm(LSys("DeleteEdition"),LSys("ConfirmQuestion"),function(response) {
                if (response) {
                    abp.ui.setBusy($body, abp.services.app.featureService.deleteEdition({
                        editionId: editionId
                    }).done(function () {
                        abp.notify.success(LSys("EditionDeleted"), LSys("Success"));

                        setTimeout(function() {
                            window.location.reload();
                        },3000);


                    }));
                }
            });
        }
    });
})();