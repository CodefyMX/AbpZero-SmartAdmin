(function () {


    var ImageUploader;

    ImageUploader = (function () {

        function ImageUploader(dialog) {
            this.canvas = document.createElement("canvas");
            this.canvas_context = this.canvas.getContext('2d');

            this.rotateImage = function (direction) {
                //These are swapped when rotating
                this.canvas.width = this.img.height;
                this.canvas.height = this.img.width;
                this.canvas_context.rotate(direction * Math.PI / 2);
                if (direction > 0) {
                    this.canvas_context.translate(0, -this.canvas.height);
                } else {
                    this.canvas_context.translate(-this.canvas.width, 0);
                }
                this.canvas_context.drawImage(this.img, 0, 0);
                this.setImageFromDataURL(this.canvas.toDataURL("image/png"), this.img.alt)
            }

            this.setImageFromDataURL = function (data_url, file_name) {
                this.image_url = data_url;
                this.img = new Image();
                this.img.src = this.image_url;
                this.img.alt = file_name;
                if (this.img.width > 720) { //Scale down to 720px
                    this.canvas.width = 720;
                    this.canvas.height = this.img.height * 720 / this.img.width;
                    this.canvas_context.drawImage(this.img, 0, 0, this.img.width, this.img.height, 0, 0, this.canvas.width, this.canvas.height);
                    return this.setImageFromDataURL(this.canvas.toDataURL("image/png"), this.img.alt);
                }
                this._dialog.populate(this.image_url, [this.img.width, this.img.height]);
            }

            this.cropImage = function (crop_region) {
                console.log(crop_region);
                this.canvas.width = this.img.width * crop_region[2];
                this.canvas.height = this.img.height * crop_region[3];
                this.canvas_context.translate(-this.img.width * crop_region[0], -this.img.height * crop_region[1]);
                this.canvas_context.drawImage(this.img, 0, 0);
                this.setImageFromDataURL(this.canvas.toDataURL("image/png"), this.img.alt);
            }


            this._dialog = dialog;
            this._dialog.addEventListener('cancel', (function (_this) {
                return function () {
                    return _this._onCancel();
                };
            })(this));
            this._dialog.addEventListener('imageuploader.cancelupload', (function (_this) {
                return function () {
                    return _this._onCancelUpload();
                };
            })(this));
            this._dialog.addEventListener('imageuploader.clear', (function (_this) {
                return function () {
                    return _this._onClear();
                };
            })(this));
            this._dialog.addEventListener('imageuploader.fileready', (function (_this) {
                return function (ev) {
                    return _this._onFileReady(ev.detail().file);
                };
            })(this));
            this._dialog.addEventListener('imageuploader.mount', (function (_this) {
                return function () {
                    return _this._onMount();
                };
            })(this));
            this._dialog.addEventListener('imageuploader.rotateccw', (function (_this) {
                return function () {
                    return _this._onRotateCCW();
                };
            })(this));
            this._dialog.addEventListener('imageuploader.rotatecw', (function (_this) {
                return function () {
                    return _this._onRotateCW();
                };
            })(this));
            this._dialog.addEventListener('imageuploader.save', (function (_this) {
                return function () {
                    return _this._onSave();
                };
            })(this));
            this._dialog.addEventListener('imageuploader.unmount', (function (_this) {
                return function () {
                    return _this._onUnmount();
                };
            })(this));
        }

        ImageUploader.prototype._onCancel = function () { };

        ImageUploader.prototype._onCancelUpload = function () {
            clearTimeout(this._uploadingTimeout);
            return this._dialog.state('empty');
        };

        ImageUploader.prototype._onClear = function () {
            this.img = null;
            return this._dialog.clear();
        };

        ImageUploader.prototype._onFileReady = function (file) {
            var _this = this;
            var reader = new FileReader();
            if (file) {
                reader.readAsDataURL(file);
                reader.addEventListener('load', function () {
                    _this.setImageFromDataURL(reader.result, file.name);
                });
            }
        };

        ImageUploader.prototype._onMount = function () { };

        ImageUploader.prototype._onRotateCCW = function () {
            this.rotateImage(-1);
        };

        ImageUploader.prototype._onRotateCW = function () {
            this.rotateImage(1);
        };

        ImageUploader.prototype._onSave = function () {
            var self = this;
            if (self._dialog.cropRegion()) {
                self.cropImage(self._dialog.cropRegion());
            }
            var payload = {};
            payload.PageId = document.getElementById("Id").value;
            payload.Lang = document.getElementById("Lang").value;
            payload.base64String = this.image_url;
            // Send the update content to the server to be saved
            abp.services.cms.pagesService.saveImageFromBase64(payload)
                .done(function (response) {
                    self._dialog.save(
                response,
                [self.img.width, self.img.height],
                {
                    'alt': self.img.alt,
                    'data-ce-max-width': self.img.width
                }
            );
                });


        };

        ImageUploader.prototype._onUnmount = function () { };

        ImageUploader.createImageUploader = function (dialog) {
            return new ImageUploader(dialog);
        };

        return ImageUploader;

    })();

    window.ImageUploader = ImageUploader;

    var editor = ContentTools.EditorApp.get();
    editor.init('*[data-editable]', 'data-name');
    console.info("Editor inicializado");
    ContentTools.StylePalette.add([
        new ContentTools.Style('Video', 'video', ['iframe']),
        new ContentTools.Style('Video Center', 'video-center', ['iframe']),
        new ContentTools.Style('Lead', 'lead', ['h1', 'h2', 'h3', 'h4', 'h5']),
        new ContentTools.Style('Small Text', 'small', ['p', 'a']),
        new ContentTools.Style('Justify', 'text-justify', ['p', 'a']),
        new ContentTools.Style('Image Centered', 'img-center', ['img'])
    ]);
    ContentTools.IMAGE_UPLOADER = ImageUploader.createImageUploader;
    var base64String;


    function capturePreview(callback) {
        html2canvas(document.body,
            {
                onrendered: function (canvas) {
                    callback(canvas);
                }
            });
    }

    editor.addEventListener('saved', function (ev) {

        var self = this;

        self.busy(true);
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
        capturePreview(function (canvas) {
            var dataUrl = canvas.toDataURL();
            base64String = dataUrl;
            // Set the editor as busy while we save our changes
            // Collect the contents of each region into a FormData instance
            var payload = {};
            payload.HtmlContent = allContent;
            payload.PageId = id;
            payload.Lang = lang;
            payload.Chunks = chunks;
            payload.base64String = base64String;
            // Send the update content to the server to be saved
            abp.ajax({
                data: JSON.stringify(payload),
                url: "/Pages/SavePage"
            }).done(function () {
                new ContentTools.FlashUI('ok');
                self.busy(false);
            }).fail(function (jqXhr, textStatus, errorThrown) {
                new ContentTools.FlashUI('no');
                self.busy(false);
            });
        });

    });

})();