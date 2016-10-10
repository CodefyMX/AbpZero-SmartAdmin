
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
    $(document)
        .ready(function () {
            $("#container")
                .jstree({
                    contextmenu: {
                        items: treeJsConfig.contextMenu
                    },
                    "checkbox": {
                        "keep_selected_style": false
                    },
                    'plugins': ["wholerow", "checkbox", "contextmenu"],
                    'core': {
                        'themes': {
                            'name': 'proton',
                            'responsive': true
                        }
                    }
                });


        });
})()