(function () {
    var editor = ContentTools.EditorApp.get();
    editor.init('*[data-editable]', 'data-name');
    ContentTools.StylePalette.add([
        new ContentTools.Style('Video', 'video', ['iframe']),
        new ContentTools.Style('Video Center', 'video-center', ['iframe']),
        new ContentTools.Style('Lead', 'lead', ['h1', 'h2', 'h3', 'h4', 'h5']),
        new ContentTools.Style('Small Text', 'small', ['p', 'a']),
        new ContentTools.Style('Justify', 'text-justify', ['p', 'a']),
        new ContentTools.Style('Image Centered', 'img-center', ['img'])
    ]);
    editor.addEventListener('saved', function (ev) {
        var allContent = document.getElementById("content").innerHTML;
        var lang = document.getElementById("Lang").value;
        var id = document.getElementById("Id").value;

        var regions = editor.orderedRegions();
        var chunks = [];
        var order = 0;
        for (name in regions) {
            var $element = $(regions[name]._domElement);
            if (regions.hasOwnProperty(name)) {
                chunks.push({
                    Key: $element.data("name"),
                    Value: regions[name]._domElement.innerHTML,
                    Order: order
                });
                order = order + 1;
            }
        }


        // Set the editor as busy while we save our changes

        var self = this;
        self.busy(true);
        // Collect the contents of each region into a FormData instance
        var payload = {};
        payload.HtmlContent = allContent;
        payload.PageId = id;
        payload.Lang = lang;
        payload.Chunks = chunks;
        console.log(payload);
        // Send the update content to the server to be saved
        abp.ajax({
            data: JSON.stringify(payload),
            url: "/Pages/SavePage"
        }).done(function () {
            new ContentTools.FlashUI('ok');
            self.busy(false);
        });
    });
    function imageUploader(dialog) {
        var image;
        dialog.addEventListener("imageuploader.clear",
            function () {
                dialog.clear();
                image = null;
            });
        dialog.addEventListener("imageuploader.fileready",
            function () {

            });
        dialog.addEventListener('imageuploader.rotateccw', function () {
            rotateImage('CCW');
        });

        dialog.addEventListener('imageuploader.rotatecw', function () {
            rotateImage('CW');
        });

        function rotateImage(direction) {

        };
    };
})();