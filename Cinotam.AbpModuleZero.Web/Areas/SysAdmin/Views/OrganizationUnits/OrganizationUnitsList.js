
(function () {
    //Todo: split page scripts



    var selectedNodeId;
    function loadUsersWindow(id) {
        var $usersWindow = $("#usersWindow");
        abp.ui.setBusy($usersWindow);

        $usersWindow.load("/SysAdmin/OrganizationUnits/UsersWindow/" + id, function () {
            abp.ui.clearBusy($usersWindow);
        });
    }
    var loadOrganizationUnitsView = function ($organizationUnitsView, jsTreeSelector, treeJsConfig) {
        abp.ui.setBusy($organizationUnitsView);
        $organizationUnitsView
            .load("/SysAdmin/OrganizationUnits/GetOrganizationUnits",
                function () {
                    abp.ui.clearBusy($organizationUnitsView);
                    var $jsTreeContainer = $(jsTreeSelector);
                    $jsTreeContainer
                        .jstree({
                            contextmenu: {
                                items: treeJsConfig.contextMenu
                            },
                            'plugins': ["wholerow", "html_data", "ui", "contextmenu", "dnd"],
                            'core': {
                                "check_callback": true,
                                'themes': {
                                    'name': 'proton',
                                    'responsive': true
                                }
                            }
                        });

                    $jsTreeContainer.on("select_node.jstree", function (evt, nodeRef) {

                        selectedNodeId = nodeRef.node.id;
                        loadUsersWindow(selectedNodeId);

                    });

                    var newParent = 0;
                    var oldPosition;
                    var newPosition;
                    var oldParent = 0;
                    $(document)
                        .on('dnd_start.vakata',
                            function (e, data) {
                                var selector = "li#" + data.data.nodes[0] + ".jstree-node";
                                oldPosition = $(selector).index();
                                oldParent = $jsTreeContainer.jstree(true).get_node(data.data.nodes[0]).parent;
                            });

                    $(document)
                        .on('dnd_stop.vakata',
                            function (e, data) {
                                var node = data.data.origin.get_node(data.data.nodes[0]);
                                if (node.type === "root") return false;
                                abp.message.confirm(LSys("MoveOrganizationUnit"),
                                    LSys("ConfirmQuestion"),
                                    function (response) {
                                        if (response) {

                                            var selector = "li#" + data.data.nodes[0] + ".jstree-node";
                                            newPosition = $(selector).index();
                                            newParent = node.parent;
                                            if (newParent === "#") {
                                                newParent = undefined;
                                            }
                                            var elementMoved = data.data.nodes[0];

                                            var request = {
                                                Id: elementMoved,
                                                ParentId: newParent

                                            }
                                            abp.services.app.organizationUnits.moveOrgUnit(request)
                                                .done(function () {
                                                    abp.notify
                                                        .success(LSys("OrganizationUnitMoved"),
                                                            LSys("Success"));
                                                });
                                        } else {
                                            $jsTreeContainer.jstree(true).move_node(node, oldParent, oldPosition);
                                            return false;
                                        }
                                        return false;
                                    });
                                return false;
                            });

                    $jsTreeContainer.on('ready.jstree', function () {
                        $jsTreeContainer.jstree("open_all");
                    });
                });
    }
    var $organizationUnitsView;
    var jsTreeSelector;
    var treeJsConfig = {};
    var $body;
    var $usersTable;
    function modalHandler(event) {
        switch (event.detail.info.modalType) {
            case "MODAL_CREATE_EDIT_ORG_UNIT":
                loadOrganizationUnitsView($organizationUnitsView, jsTreeSelector, treeJsConfig);
                abp.notify.success(LSys("OrganizationUnitCreated"), LSys("Success"));
                break;
            case "MODAL_USER_ADDED":
                abp.notify.success(LSys("UserAdded"), LSys("Success"));
                loadUsersWindow(selectedNodeId);
                break;
            default:
                console.log("Event unhandled");
        }
    }

    $(document)
        .ready(function () {

            $organizationUnitsView = $("#organizationUnitsView");
            jsTreeSelector = "#container";

            $body = $("body");
            $usersTable = $("#usersTable");
            var _organizationUnitsAppService = abp.services.app.organizationUnits;

            var contextMenu = function (node) {
                var items = {
                    addUnit: {
                        label: LSys('AddOrganizationUnit'),
                        action: function () {
                            window.modalInstance.open("/SysAdmin/OrganizationUnits/AddOrganizationUnit/" + node.id);
                        }
                    },
                    editUnit: {
                        label: LSys('EditOrganizationUnit'),
                        action: function () {
                            window.modalInstance.open("/SysAdmin/OrganizationUnits/CreateEditOrganizationUnit/" +
                                node.id);
                        }
                    },
                    deleteUnit: {
                        label: LSys('DeleteOrganizationUnit'),
                        action: function () {
                            abp.message.confirm(LSys("TheUnitWillBeDeleted"), LSys("ConfirmQuestion"), function (response) {
                                if (response) {
                                    _organizationUnitsAppService.removeOrganizationUnit(node.id).done(function () {
                                        abp.notify.success(LSys("OrganizationUnitRemoved"), LSys("Success"));
                                        loadOrganizationUnitsView($organizationUnitsView, jsTreeSelector, treeJsConfig);
                                    });
                                }
                            });
                        }
                    }
                }

                return items;
            }
            treeJsConfig = {
                contextMenu: contextMenu,
                modalHandler: modalHandler
            }

            document.addEventListener('modalClose', treeJsConfig.modalHandler);
            loadOrganizationUnitsView($organizationUnitsView, jsTreeSelector, treeJsConfig);
            $body.on("click", ".js-remove", function () {
                var userId = $(this).data("user-id");
                var orgId = $(this).data("org-id");

                abp.message.confirm(LSys("UserWillBeRemovedFromOrganizationUnit"), LSys("ConfirmQuestion"), function (response) {
                    if (response) {
                        abp.ui.setBusy($usersTable, _organizationUnitsAppService.removeUserFromOrganizationUnit({
                            UserId: userId,
                            OrgUnitId: orgId
                        }).done(function () {
                            abp.notify.success(LSys("UserRemovedFromOrganizationUnit"), LSys("Success"));
                            loadUsersWindow(selectedNodeId);
                        }));
                    }
                });

            });

        });

})();