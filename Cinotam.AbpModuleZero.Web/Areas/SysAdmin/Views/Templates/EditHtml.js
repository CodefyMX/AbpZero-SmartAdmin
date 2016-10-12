(function () {
    $(document)
        .ready(function () {
            var editor = ace.edit("editor");
            editor.setTheme("ace/theme/twilight");
            editor.session.setMode("ace/mode/html");

            var $form =$("#editHtml");

                $form.on("submit",
                    function (e) {
                        var $self = $(this);
                        e.preventDefault();
                        var data = $self.serializeFormToObject();
                        data.Content = editor.getValue();
                        abp.services.cms.templateService.addTemplate(data).done(function () {
                            abp.message.success(LSys("Saved"), LSys("Success"));
                        });
                    });
        });
})();
