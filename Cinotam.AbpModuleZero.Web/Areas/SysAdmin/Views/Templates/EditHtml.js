(function () {
    $(document)
        .ready(function () {
            var editor = ace.edit("editor");
            editor.setTheme("ace/theme/twilight");
            editor.session.setMode("ace/mode/html");
            $("#editHtml")
                .on("submit",
                    function (e) {
                        e.preventDefault();
                        var data = $(this).serializeFormToObject();
                        data.Content = editor.getValue();
                        abp.services.cms.templateService.addTemplate(data).done(function () {
                            abp.message.success(LSys("Saved"), LSys("ContentSaved"));
                        });
                    });
        });
})();
