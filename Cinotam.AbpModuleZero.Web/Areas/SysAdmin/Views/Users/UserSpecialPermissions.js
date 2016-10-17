(function () {
    var modalType = "MODAL_PERMISSIONS_SET";
    $(document).ready(function () {

        var _userAppService = abp.services.app.user;
        var $container = $("#container");
        var $form = $("#setPermissions");
        var userId = $("#UserId").val();
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

        var resetBtn = $(".js-reset-permissions");
        resetBtn.click(function () {
            
            abp.ui.setBusy($form, _userAppService.resetAllPermissions(userId).done(function () {
                window.modalInstance.close({}, modalType);
            }));
        });
        $form.on("submit", function (e) {
            var data = {
                AssignedPermissions: [],
                UserId: userId
            }
            e.preventDefault();
            var selected = $container.jstree('get_json');
            $(selected).each(function (index, v) {
                var granted = v.state.selected;

                data.AssignedPermissions.push({
                    Name: v.id,
                    Granted: granted
                });

                getPermissionsForChildren(v.children);
                

            });
            abp.ui.setBusy($form, _userAppService.setUserSpecialPermissions(data).done(function () {
                window.modalInstance.close({}, modalType);
            }));

            function getPermissionsForChildren(children) {
                children.forEach(function(v) {
                    var granted = v.state.selected;
                    data.AssignedPermissions.push({
                        Name: v.id,
                        Granted: granted
                    });
                    getPermissionsForChildren(v.children);
                });
            }

        });

    });
})();