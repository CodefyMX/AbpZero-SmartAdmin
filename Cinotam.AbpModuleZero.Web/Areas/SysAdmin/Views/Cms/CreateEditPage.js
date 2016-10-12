(function () {
    $(document)
        .ready(function () {
            var _pagesAppService = abp.services.cms.pagesService;
            $("#createPageForm").on("submit", function (e) {
                e.preventDefault();
                var data = $(this).serializeFormToObject();
                _pagesAppService.createEditPage(data)
                    .done(function () {
                        window.location.reload();
                    });
            });
        });

})();