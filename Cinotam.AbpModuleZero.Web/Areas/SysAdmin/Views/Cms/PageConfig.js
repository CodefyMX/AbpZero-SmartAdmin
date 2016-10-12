(function () {

    var initialize = function (_pagesAppService) {
        var pageId = $("#Id").val();

        var $toggleStatusBtn = $('.js-toggle-status');
        var $toggleMainPageStatus = $('.js-toggle-main');
        var $toggleInMenuStatus = $('.js-toggle-menu');
        var $deletePageBtn = $(".js-delete-page");
        var $pageOptionsView = $("#pageOpts");
        var $categorySelect = $("#CategoryId");
        var $parentSelect = $("#ParentId");
        var $templateSelect = $("#TemplateName");
        $toggleStatusBtn.click(function () {
            abp.ui.setBusy($pageOptionsView, _pagesAppService.togglePageStatus(pageId)
                    .done(function () {
                        window.location.reload();
                    }));
        });

        $toggleMainPageStatus.change(function () {
            abp.ui.setBusy($pageOptionsView, _pagesAppService.setPageAsMain(pageId)
                .done(function () {

                }));
        });

        $toggleInMenuStatus.change(function () {
            abp.ui.setBusy($pageOptionsView, _pagesAppService.togglePageInMenuStatus(pageId)
                .done(function () {

                }));
        });
        $deletePageBtn.click(function () {
            var $self = $(this);
            var id = $self.data("id");
            abp.message.confirm(LSys("ElementWillBeDeleted"), LSys("ConfirmQuestion"), function (response) {
                if (response) {
                    _pagesAppService.deletePage(id)
                        .done(function () {
                            window.location.href = "/SysAdmin/Cms/MyPages";
                        });
                }
            });

        });

        $categorySelect.on("change", function (e) {
            e.preventDefault();
            var categoryId = $categorySelect.val();
            if (categoryId === 0) {
                categoryId = null;
            }
            var selectedCategory = {
                CategoryId: categoryId,
                PageId: pageId
            }
            abp.ui.setBusy(this, _pagesAppService.setCategory(selectedCategory)
                .done(function () {
                    abp.notify.success(LSys("CategoryChanged"));
                }));
        });
        
        $parentSelect.on("change", function (e) {
                var $self = $(this);
               e.preventDefault();
               var parentPage = $parentSelect.val();
               if (parentPage === 0) {
                   parentPage = null;
               }
               var selectedParent = {
                   ParentPageId: parentPage,
                   PageId: pageId
               }
               abp.ui.setBusy($self, _pagesAppService.setParentPage(selectedParent)
                   .done(function () {
                       abp.notify.success(LSys("ParentChanged"));
                   }));
           });
        
        $templateSelect.on("change", function () {
               var templateName = $templateSelect.val();

               var selectedTemplate = {
                   TemplateName: templateName,
                   PageId: pageId
               }
               abp.ui.setBusy(this, _pagesAppService.setTemplate(selectedTemplate)
                   .done(function () {
                       abp.notify.warn(LSys("TemplateChanged"));
                       setTimeout(function () {
                           window.location.reload();
                       }, 2000);
                   }));
           });

    }
    var pageConfig = {
        Initialize: initialize
    }
    $(document)
        .ready(function () {

            var _pagesAppService = abp.services.cms.pagesService;
            pageConfig.Initialize(_pagesAppService);
        });
})();