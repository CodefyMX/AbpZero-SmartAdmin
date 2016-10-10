(function () {
    var pageId = $("#Id").val();
    $('.js-toggle-status')
        .click(function () {
            abp.ui.setBusy($("#pageOpts"), abp.services.cms.pagesService.togglePageStatus(pageId)
                .done(function () {
                    window.location.reload();
                }));
        });
    $('.js-toggle-main')
        .change(function () {
            abp.ui.setBusy($("#pageOpts"), abp.services.cms.pagesService.setPageAsMain(pageId)
                .done(function () {

                }));
        });
    $('.js-toggle-menu')
        .change(function () {
            abp.ui.setBusy($("#pageOpts"), abp.services.cms.pagesService.togglePageInMenuStatus(pageId)
                .done(function () {

                }));
        });
    $(".js-delete-page").click(function () {
        var id = $(this).data("id");
        abp.message.confirm(LSys("Sure"), LSys("ElementWillBeDeleted"), function (response) {
            if (response) {
                abp.services.cms.pagesService.deletePage(id)
                    .done(function () {
                        window.location.href = "/SysAdmin/Cms/MyPages";
                    });
            }
        });

    });
    $("#CategoryId")
        .on("change", function (e) {
            e.preventDefault();
            var categoryId = $("#CategoryId").val();
            if (categoryId === 0) {
                categoryId = null;
            }
            var selectedCategory = {
                CategoryId: categoryId,
                PageId: $("#Id").val()
            }
            abp.ui.setBusy(this, abp.services.cms.pagesService.setCategory(selectedCategory)
                .done(function () {
                    abp.notify.success(LSys("CategoryChanged"));
                }));
        });
    $("#ParentId")
       .on("change", function (e) {
           e.preventDefault();
           var parentPage = $("#ParentId").val();
           if (parentPage === 0) {
               parentPage = null;
           }
           var selectedParent = {
               ParentPageId: parentPage,
               PageId: $("#Id").val()
           }
           abp.ui.setBusy(this, abp.services.cms.pagesService.setParentPage(selectedParent)
               .done(function () {
                   abp.notify.success(LSys("ParentChanged"));
               }));
       });
    $("#TemplateName")
       .on("change", function (e) {
           var templateName = $("#TemplateName").val();

           var selectedTemplate = {
               TemplateName: templateName,
               PageId: $("#Id").val()
           }
           abp.ui.setBusy(this, abp.services.cms.pagesService.setTemplate(selectedTemplate)
               .done(function () {
                   abp.notify.warn(LSys("TemplateChanged"));
                   setTimeout(function () {
                       window.location.reload();
                   }, 2000);
               }));
       });

})();