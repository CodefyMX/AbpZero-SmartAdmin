
(function () {
    $(document).ready(function () {
        bind();
    });


    $("body")
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

    window.bind = function () {
        $('#menus').sortable({
            handle: ".handle",
            orientation: "y",
            update: function () {
                var data = [];
                $("#menus li").each(function (index) {
                    var id = $(this).data("id");
                    data.push({
                        Order: index,
                        Id: id
                    });
                });
                ChangeOrder(data);
            }
        });
    }
    function ChangeOrder(data) {
        abp.ui.setBusy($("#menus"), abp.services.cms.menuService.changeOrder(data, "Menu").done(function () {
            abp.notify.success(LSys("OrderChanged"));
        }));
    }
})();