
(function () {
    $(document).ready(function () {
        var $body = $("body");
        var $menus = $("#menus");
        var $menusLi = $("#menus li");
        bind($menus, $menusLi);
        $body
        .on("click",
            ".js-delete-menu",
            function () {
                var id = $(this).data("id");

                abp.message.confirm(LSys("ElementWillBeRemoved"), LSys("Sure"), function (response) {

                    if (response) {
                        abp.services.cms.menuService.deleteMenu(id)
                            .done(function () {

                                window.location.reload();

                            });
                    }

                });

            });
    });


    

    window.bind = function ($menus,$menusLi) {
        
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
                changeOrder(data, $menus);
            }
        });
    }
    function changeOrder(data,$menus) {
        abp.ui.setBusy($menus, abp.services.cms.menuService.changeOrder(data, "Menu").done(function () {
            abp.notify.success(LSys("OrderChanged"));
        }));
    }
})();