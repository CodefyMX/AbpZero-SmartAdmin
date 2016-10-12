(function () {
    $(document)
        .ready(function () {

            var _pagesAppService = abp.services.cms.pagesService;

            $("#setPageName").on("submit", function (e) {
                e.preventDefault();
                var data = $(this).serializeFormToObject();
                _pagesAppService.createEditPageTitle(data)
                    .done(function () {
                        window.location.reload();
                    });
            });
        });

})();