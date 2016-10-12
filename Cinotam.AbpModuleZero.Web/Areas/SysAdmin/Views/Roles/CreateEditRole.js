
(function () {
    var modalType = "MODAL_ROLE_CREATED";
    $(document).ready(function () {

        var _roleAppService = abp.services.app.role;
        var $container = $("#container");
        var $form = $("#createEditRole");
        $container
            .jstree({
                "checkbox": {
                    keep_selected_style: false,
                    three_state: false,
                    cascade: ''
                },
                'plugins': ["wholerow", "html_data", "checkbox", "ui"],
                'core': {

                    "multiple": true,
                    'themes': {
                        'name': 'proton',
                        'responsive': true
                    }
                }
            });
        $container.on('ready.jstree', function () {
            $container.jstree("open_all");
        });

        $container.on("changed.jstree", function (e, data) {
            if (!data.node) {
                return;
            }

            var childrenNodes;

            if (data.node.state.selected) {
                selectNodeAndAllParents($container.jstree('get_parent', data.node));

                childrenNodes = $.makeArray($container.jstree('get_children_dom', data.node));
                $container.jstree('select_node', childrenNodes);

            } else {
                childrenNodes = $.makeArray($container.jstree('get_children_dom', data.node));
                $container.jstree('deselect_node', childrenNodes);
            }
        });
        function selectNodeAndAllParents(node) {
            $container.jstree('select_node', node, true);
            var parent = $container.jstree('get_parent', node);
            if (parent) {
                selectNodeAndAllParents(parent);
            }
        };


        $form.on("submit", function (e) {
            var data = {
                AssignedPermissions: [],
                DisplayName: $("#DisplayName").val(),
                Id: $("#Id").val(),
                IsDefault :$("#IsDefault").is(":checked")
            }
            e.preventDefault();
            var selected = $container.jstree('get_selected');
            $(selected).each(function (index, v) {
                console.log(index);
                data.AssignedPermissions.push({
                    Name: v,
                    Granted: true
                });
            });
            abp.ui.setBusy($form, _roleAppService.createEditRole(data).done(function () {
                window.modalInstance.close({}, modalType);
            }));
        });

    });
})();