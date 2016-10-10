
(function () {
    $(document)
        .ready(function () {
            $("#createTemplateForm").on("submit", function (e) {
                e.preventDefault();
                var data = $(this).serializeFormToObject();
                data.IsPartial = $("#IsPartial").is(":checked");
                console.log(data);
                abp.services.cms.templateService.addTemplate(data)
                    .done(function () {
                        window.location.reload();
                    });
            });
        });

})();