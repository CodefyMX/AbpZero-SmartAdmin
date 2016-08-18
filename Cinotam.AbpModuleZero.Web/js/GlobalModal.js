//El Peri!
(function () {
    var modal = function (container) {
        var modalTypes = {
            MODAL_CANCEL: 'MODAL_CANCEL'
        }
        var modalConfig = {
            show: true,
            backdrop: 'static',
            keyboard: false
        }
        var modalInstance = {};
        modalInstance.modalCloseEvent = {
        };
        var selfModal = this;
        if (container) {
            var isJquery = container instanceof $;
            if (isJquery) {
                selfModal.container = container;
            } else {
                selfModal.container = $(container);
            }
        } else {
            console.log("Modal not defined loading default");
            selfModal.container = $('#modal');
        }

        selfModal.initModal = function () {
            selfModal.container.modal(modalConfig);
        }
        modalInstance.open = function (url, data) {
            if (url) {
                selfModal.container.load(url, data, function () {
                    selfModal.initModal();
                });
            }
        }
        modalInstance.close = function (data, modalType) {
            selfModal.container.modal('hide');
            data.modalType = modalType;
            var modalCloseEvent = new CustomEvent('modalClose', {
                detail: {
                    info: data
                },
                bubbles: true,
                cancelable: false
            });
            document.dispatchEvent(modalCloseEvent);
        }
        function initListener() {
            console.log('Modal service beep awaiting orders... bep bep');
            $('body').on('click', '[data-modal]', function (e) {
                e.preventDefault();
                var url = $(this).data('url') || $(this).attr('href');
               
                if (url) {
                    console.log(url);

                    selfModal.container.load(url, function () {
                        selfModal.initModal();
                    });
                }
            });
            $('body').on('click', '[data-cancel]', function (e) {
                e.preventDefault();
                modalInstance.close({
                }, modalTypes.MODAL_CANCEL);
            });
        }
        initListener();
        return modalInstance;
    };
    var nameSpace = abp.utils.createNamespace(abp, 'app.bootstrap');
    nameSpace.modal = modal;
}) ();

