
(function () {
    $(document)
        .ready(function () {
            var contextMenu = function () {
                var items = {
                    editUnit: {
                        label: LSys('EditValue'),
                        action: function (data) {
                            console.log(data);
                        }
                    }
                }

                return items;
            }
            var treeJsConfig = {
                contextMenu: contextMenu

            }
            var jsTreeInstance;
            var $jsTree = $('#container');
            var $form = $("#createEditEdition");
            var _featureAppService = abp.services.app.featureService;
            jsTreeInstance = $jsTree
                .jstree({
                    "checkbox": {
                        keep_selected_style: false,
                        three_state: false,
                        cascade: ''
                    },
                    contextmenu: {
                        items: treeJsConfig.contextMenu
                    },
                    'plugins': ["checkbox"],
                    'core': {
                        'themes': {
                            'name': 'proton',
                            'responsive': true
                        }
                    }
                });
            $jsTree.on('ready.jstree', function () {
                var $tree = $(this);
                $($tree.jstree().get_json($tree))
                  .each(function (index, value) {
                      var selector = "#" + value.id;
                      var jqueryElement = $(selector);
                      var requiresTextBox = jqueryElement.data("append-textbox");

                      if (requiresTextBox) {

                          jqueryElement.append("<input type='text' class='input-tree' />");
                      }
                  });


                $jsTree.jstree("open_all");
            });
            $jsTree.on("changed.jstree", function (e, data) {
                if (!data.node) {
                    return;
                }

                var childrenNodes;

                if (data.node.state.selected) {
                    selectNodeAndAllParents($("#container").jstree('get_parent', data.node));

                    childrenNodes = $.makeArray($("#container").jstree('get_children_dom', data.node));
                    $jsTree.jstree('select_node', childrenNodes);

                } else {
                    childrenNodes = $.makeArray($("#container").jstree('get_children_dom', data.node));
                    $jsTree.jstree('deselect_node', childrenNodes);
                }
            });
            function selectNodeAndAllParents(node) {
                $jsTree.jstree('select_node', node, true);
                var parent = $("#container").jstree('get_parent', node);
                if (parent) {
                    selectNodeAndAllParents(parent);
                }
            };
            $jsTree.on('open_node.jstree', function (evt, nodeRef) {
                nodeRef.node.children.forEach(function (i) {
                    printTextBoxIfNeededForNodeNames(i);
                });
            });
            function printTextBoxIfNeededForNodeNames(name) {
                var node = getNode(name);
                var selector = "#" + name;
                var jqueryElement = $(selector);
                var requiresTextBox = jqueryElement.data("append-textbox");
                if (requiresTextBox) {
                    var defaultValue = jqueryElement.data("value");
                    removeCheckBoxFromNode(selector, jqueryElement);
                    jqueryElement.append("<input type='text' value=" + defaultValue + " class='input-tree' data-text-id='" + name + "' />");
                }
                node.children.forEach(function (i) {
                    printTextBoxIfNeeded(i);
                });
            }
            function removeCheckBoxFromNode(selector, jqueryElement) {
                var anchorElement = $(selector + "_anchor");
                anchorElement.attr("class", "simple-text");
                var anchorElementCheckBox = jqueryElement.find(".jstree-icon.jstree-checkbox");
                var anchorElementIcon = jqueryElement.find(".jstree-icon.jstree-themeicon");
                anchorElementCheckBox.remove();
                anchorElementIcon.remove();
            }
            function getNode(id) {
                return $.jstree.reference(jsTreeInstance).get_node(id);  // use the tree reference to fetch a node
            }
            function printTextBoxIfNeeded(value) {

                var selector = "#" + value.id;
                var jqueryElement = $(selector);
                var requiresTextBox = jqueryElement.data("append-textbox");
                if (requiresTextBox) {
                    removeCheckBoxFromNode(selector, jqueryElement);
                    var checked = jqueryElement.data("selected");
                    console.log(checked);
                    if (checked === "True") {
                        $jsTree.jstree("check_node", selector);
                    } else {
                        $jsTree.jstree("uncheck_node", selector);
                    }
                    var defaultValue = jqueryElement.data("value");
                    jqueryElement.append("<input type='text' value=" + defaultValue + " class='input-tree' data-text-id='" + name + "' />");
                }
                value.children.forEach(function (i) {
                    printTextBoxIfNeeded(i);
                });
            }

            $form
                .on("submit",
                    function (e) {
                        var formData = $(this).serializeFormToObject();
                        e.preventDefault();
                        var features = [];
                        var selected = $jsTree.jstree('get_json');
                        $(selected).each(function (index, v) {
                            var value = v.state.selected;
                            var selectedStatus = isAnyChildrenSelected(v.children);
                            if (value === false) {
                                value = selectedStatus;
                            } else {
                                selectedStatus = value;
                            }
                            var textBox = $(document).find("[data-text-id='" + v.id + "']");
                            if (textBox.length === 1) {

                                value = $(textBox[0]).val();

                                selectedStatus = true;


                            }
                            features.push({
                                Name: v.id,
                                DefaultValue: value,
                                Selected: selectedStatus
                            });

                            getFeaturesForChildren(v.children);


                        });

                        function isAnyChildrenSelected(children) {

                            for (var i = 0; i < children.length; i++) {
                                if (children[i].state.selected) {
                                    return true;
                                }
                            }
                            return false;
                        }

                        function getFeaturesForChildren(children) {
                            children.forEach(function (v) {
                                var value = v.state.selected;
                                var selectedStatus = isAnyChildrenSelected(v.children);
                                if (value === false) {
                                    value = selectedStatus;
                                } else {
                                    selectedStatus = value;
                                }
                                var textBox = $(document).find("[data-text-id='" + v.id + "']");
                                if (textBox.length === 1) {
                                    value = $(textBox[0]).val();
                                    selectedStatus = true;
                                }
                                features.push({
                                    Name: v.id,
                                    DefaultValue: value,
                                    Selected: selectedStatus
                                });

                                getFeaturesForChildren(v.children);

                            });
                        }
                        formData.Features = features;

                        abp.ui.setBusy($form, _featureAppService.createEdition(formData).done(function () {
                            window.location.reload();
                        }));

                    });

        });
})()
