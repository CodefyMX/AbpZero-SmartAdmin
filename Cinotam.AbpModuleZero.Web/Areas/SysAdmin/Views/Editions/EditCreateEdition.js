
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
    $(document)
        .ready(function () {
            $("#container")
                .jstree({
                    contextmenu: {
                        items: contextMenu
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