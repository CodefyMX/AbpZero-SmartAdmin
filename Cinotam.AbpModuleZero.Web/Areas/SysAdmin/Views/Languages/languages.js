(function () {
    "use strict";
    $(document)
        .ready(function () {

            var _languageAppService = abp.services.app.language;
            var $table = $("#languagesTable");
            var $body = $("body");
            var languageTextEditionGranted = abp.auth.isGranted("Pages.SysAdminLanguages.ChangeTexts");
            var languageDeleteGranted = abp.auth.isGranted("Pages.SysAdminLanguages.Delete");
            var columns = [
            {
                "data": "Id"
            },
                    {
                        "data": "DisplayName"
                    },
                    { "data": "CreationTimeString" }
            ];

            var columnDef = [
                    {
                        "render": function (data, type, row) {


                            var changeTextBtn = "";
                            var deleteLangBtn = "";
                            if (languageTextEditionGranted) {
                                changeTextBtn = "<a href='/SysAdmin/Languages/GetLanguageTexts?targetLang=" +
                                row.Name +
                                "' class='btn btn-default btn-xs' title='" +
                                LSys("EditTexts") +
                                "' ><i class='fa fa-edit'></i></a>";
                            }

                            if (languageDeleteGranted) {
                                deleteLangBtn = " <a data-name='" + row.DisplayName + "' data-code=" + row.Name + " class='btn btn-default btn-xs js-delete-language' title='" + LSys("DeleteLanguage") + "' ><i class='fa fa-trash'></i></a>";
                            }

                            if (row.IsStatic) {
                                if (abp.session.tenantId == null) {
                                    return changeTextBtn + deleteLangBtn;
                                }
                                return changeTextBtn;
                            } else {
                                return changeTextBtn + deleteLangBtn;
                            }
                        },
                        "targets": 0
                    },
                    {
                        "name": "CreationTime",
                        "targets": 2
                    },
                    {
                        "render": function (data, type, row) {
                            return "<i class=" + row.Icon + "></i> " + row.DisplayName;
                        },
                        "targets": 1
                    },
                    {
                        "render": function (data, type, row) {
                            if (row.IsStatic) {
                                return "<span class='label label-default'>" + LSys("Static") + "</span>";
                            } else {
                                return "<span class='label label-primary'>" + LSys("NoStatic") + "</span>";
                            }
                        },
                        "targets": 3
                    }

            ];

            var dataTablesConfig = new DatatablesConfig({
                Columns: columns,
                ColumnDefinitions: columnDef,
                Element: $table,
                OnInitComplete: function () { },
                Url: "/SysAdmin/Languages/" + "LoadLanguages",
                DisplayLength: 10
            });
            var table;
            var languagePageConfig = {
                dataTablesConfig: dataTablesConfig,
                eventHandler: function modalHandler(event) {
                    switch (event.detail.info.modalType) {
                        case "LANGUAGE_CREATED":
                            table.ajax.reload();
                            abp.notify.success("Lenguaje creado", "¡Exito!");
                            break;
                        default:
                            console.log("Event unhandled");
                    }
                }
            }

            table = $table
                .DataTable(languagePageConfig.dataTablesConfig);

            $body.on('click', '.js-delete-language', function () {
                var code = $(this).data('code');
                var name = $(this).data('name');

                abp.message.confirm(abp.utils.formatString(LSys("DeleteLanguageMessage"), name), LSys("ConfirmQuestion"), function (response) {
                    if (response) {
                        abp.ui.setBusy($body, _languageAppService.deleteLanguage(code).done(function () {
                            table.ajax.reload();
                            //Todo: translate
                            abp.notify.warn("Lenguaje [" + name + "] eliminado", "¡Exito!");
                        }));
                    }
                });
            });
            document.addEventListener('modalClose', languagePageConfig.eventHandler);
        });
})();