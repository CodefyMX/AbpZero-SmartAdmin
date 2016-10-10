
(function () {
    var contextMenu = function (node) {
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
    $(document)
        .ready(function () {
            jsTreeInstance = $("#container")
                .jstree({
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
            $('#container').on('ready.jstree', function () {
                var $tree = $(this);
                $($tree.jstree().get_json($tree))
                  .each(function (index, value) {
                      var selector = "#" + value.id;
                      var jqueryElement = $(selector);
                      var requiresTextBox = jqueryElement.data("append-textbox");
                      var checked = jqueryElement.data("selected");
                      console.log(checked);
                      if (checked == "True") {
                          $("#container").jstree("check_node", selector);
                      } else {
                          $("#container").jstree("uncheck_node", selector);
                      }
                      if (requiresTextBox) {

                          jqueryElement.append("<input type='text' class='input-tree' />");
                      }
                  });


                $("#container").jstree("open_all");
            });

            $("#container").on('open_node.jstree', function (evt, nodeRef) {
                nodeRef.node.children.forEach(function (i, v) {
                    printTextBoxIfNeededForNodeNames(i);
                });
            });
            function printTextBoxIfNeededForNodeNames(name) {
                var node = getNode(name);
                var selector = "#" + name;
                var jqueryElement = $(selector);
                var requiresTextBox = jqueryElement.data("append-textbox");
                if (requiresTextBox) {

                    //var anchorElement = $(selector + "_anchor");
                    //anchorElement.attr("id", "");
                    //anchorElement.attr("class", "simple-text");
                    //var anchorElementCheckBox = jqueryElement.find(".jstree-icon.jstree-checkbox");
                    //var anchorElementIcon = jqueryElement.find(".jstree-icon.jstree-themeicon");
                    //anchorElementCheckBox.remove();
                    //anchorElementIcon.remove();
                    var defaultValue = jqueryElement.data("value");
                    var checked = jqueryElement.data("selected");
                    console.log(checked);
                    if (checked == "True") {
                        $("#container").jstree("check_node", selector);
                    } else {
                        $("#container").jstree("uncheck_node", selector);
                    }
                    jqueryElement.append("<input type='text' value=" + defaultValue + " class='input-tree' data-text-id='" + name + "' />");
                }
                node.children.forEach(function (i, v) {
                    printTextBoxIfNeeded(i);
                });
            }
            function getNode(id) {
                return $.jstree.reference(jsTreeInstance).get_node(id);  // use the tree reference to fetch a node
            }
            function printTextBoxIfNeeded(value) {

                var selector = "#" + value.id;
                var jqueryElement = $(selector);
                var requiresTextBox = jqueryElement.data("append-textbox");
                if (requiresTextBox) {
                    //var anchorElements = jqueryElement.find(".jstree-icon.jstree-checkbox");
                    //console.log(anchorElements);
                    var checked = jqueryElement.data("selected");
                    console.log(checked);
                    if (checked == "True") {
                        $("#container").jstree("check_node", selector);
                    } else {
                        $("#container").jstree("uncheck_node", selector);
                    }
                    var defaultValue = jqueryElement.data("value");
                    jqueryElement.append("<input type='text' value=" + defaultValue + " class='input-tree' data-text-id='" + name + "' />");
                }
                value.children.forEach(function (i, v) {
                    printTextBoxIfNeeded(i);
                });
            }

            $("#createEditEdition")
                .on("submit",
                    function (e) {
                        var formData = $(this).serializeFormToObject();
                        e.preventDefault();
                        var features = [];
                        var selected = $("#container").jstree('get_json');
                        $(selected).each(function (index, v) {
                            var value = v.state.selected;
                            console.log("Selected status for: " + v.id + "", value);
                            var selectedStatus = isAnyChildrenSelected(v.children);
                            console.log("Has childrens activated", selectedStatus);
                            if (value === false) {
                                value = selectedStatus;
                            } else {
                                selectedStatus = value;
                            }
                            var textBox = $(document).find("[data-text-id='" + v.id + "']");
                            if (textBox.length === 1) {

                                var valueHolder = value;

                                value = $(textBox[0]).val();

                                if (!valueHolder) {
                                    selectedStatus = false;
                                }

                            }

                            console.log("Final check--Selected status for: " + v.id + "", value);

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
                                var textBox = $(document).find("[data-text-id='" + v.id + "']");
                                if (textBox.length === 1) {
                                    value = $(textBox[0]).val();
                                }


                                features.push({
                                    Name: v.id,
                                    DefaultValue: value,
                                    Selected: v.state.selected
                                });

                                getFeaturesForChildren(v.children);

                            });
                        }
                        console.log("Features", features);
                        formData.Features = features;

                        abp.ui.setBusy("#createEditEdition", abp.services.app.featureService.createEdition(formData).done(function () {
                            window.location.reload();
                        }));

                    });

        });
})()