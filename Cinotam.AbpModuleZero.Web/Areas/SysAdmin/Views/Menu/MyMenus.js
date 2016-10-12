
(function () {
    $(document).ready(function () {

        var _menuAppService = abp.services.cms.menuService;
        var $body = $("body");
        var $menus = $("#menus");
        var $menusLi = $("#menus li");
        bind($menus, $menusLi, _menuAppService);
        $body
        .on("click",
            ".js-delete-menu",
            function () {
                var id = $(this).data("id");

                abp.message.confirm(LSys("ElementWillBeRemoved"), LSys("Sure"), function (response) {

                    if (response) {
                        _menuAppService.deleteMenu(id)
                            .done(function () {

                                window.location.reload();

                            });
                    }

                });

            });
    });


    

    window.bind = function ($menus,$menusLi,_menuAppService) {
        
        $menus.sortable({
            handle: ".handle",
            orientation: "y",
            update: function () {
                var data = [];
                $menusLi.each(function (index) {
                    var id = $(this).data("id");
                    data.push({
                        Order: index,
                        Id: id
                    });
                });
                changeOrder(data, $menus, _menuAppService);
            }
        });
    }
    function changeOrder(data,$menus,_menuAppService) {
        abp.ui.setBusy($menus, _menuAppService.changeOrder(data, "Menu").done(function () {
            abp.notify.success(LSys("OrderChanged"));
        }));
    }
})();