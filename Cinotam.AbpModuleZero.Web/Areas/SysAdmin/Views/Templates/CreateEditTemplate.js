
(function () {
    
    $(document)
        .ready(function () {
            var _templateAppService = abp.services.cms.templateService;
            var $form = $("#createTemplateForm");
            $form.on("submit", function (e) {
                var $self = $(this);
                e.preventDefault();
                var data = $self.serializeFormToObject();
                data.IsPartial = $("#IsPartial").is(":checked");
                _templateAppService.addTemplate(data)
                    .done(function () {
                        window.location.reload();
                    });
            });
        });

})();