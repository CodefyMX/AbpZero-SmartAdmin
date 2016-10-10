(function () {
    $(document)
        .ready(function () {
            $("#setPageName").on("submit", function (e) {
                e.preventDefault();
                var data = $(this).serializeFormToObject();
                abp.services.cms.pagesService.createEditPageTitle(data)
                    .done(function () {
                        window.location.reload();
                    });
            });
        });

})();