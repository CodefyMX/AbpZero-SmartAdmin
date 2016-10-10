(function () {
    $(document)
        .ready(function () {
            $("#createPageForm").on("submit", function (e) {
                e.preventDefault();
                var data = $(this).serializeFormToObject();
                console.log(data);
                abp.services.cms.pagesService.createEditPage(data)
                    .done(function () {
                        window.location.reload();
                    });
            });
        });

})();