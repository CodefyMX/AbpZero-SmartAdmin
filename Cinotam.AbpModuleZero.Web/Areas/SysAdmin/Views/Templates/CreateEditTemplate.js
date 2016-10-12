
(function () {
    $(document)
        .ready(function () {
            var $form = $("#createTemplateForm");
            $form.on("submit", function (e) {
                var $self = $(this);
                e.preventDefault();
                var data = $self.serializeFormToObject();
                data.IsPartial = $("#IsPartial").is(":checked");
                abp.services.cms.templateService.addTemplate(data)
                    .done(function () {
                        window.location.reload();
                    });
            });
        });

})();