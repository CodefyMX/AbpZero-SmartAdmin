(function () {
    $(function () {

        var _tenantService = abp.services.app.tenant;
        var _$modal = $('#TenantCreateModal');
        var _$form = _$modal.find('form');

        _$form.validate();

        _$form.find('button[type="submit"]').click(function (e) {
            e.preventDefault();

            if (!_$form.valid()) {
                return;
            }

            var tenant = _$form.serializeFormToObject(); //serializeFormToObject is defined in main.js

            abp.ui.setBusy(_$modal);
            _tenantService.createTenant(tenant).done(function () {
                _$modal.modal('hide');
                location.reload(true); //reload page to see new tenant!
            }).always(function () {
                abp.ui.clearBusy(_$modal);
            });
        });

        _$modal.on('shown.bs.modal', function () {
            _$modal.find('input:not([type=hidden]):first').focus();
        });




        var columns = [

            { "data": "Id" },
            {
                "data": "Name"
            },
            {
                "data": "TenancyName"
            }
        ];
        var columnDefinitions = [
            {
                "render": function (data, type, row) {

                    var editionBtn = "";

                    var setFeaBtn = "";

                    var statsBtn = "";

                    var delBtn = "";


                    editionBtn = '<a data-modal href="/SysAdmin/Tenants/SetTenantEdition?tenantId=' +
                        row.Id +
                        '" class="btn btn-default btn-xs" title="' + LSys("SetEdition") + '"><i class="fa fa-object-group"></i></a> ';


                    if (row.EditionId != 0) {
                        setFeaBtn = '<a data-modal href="/SysAdmin/Tenants/SetTenantFeatures?tenantId=' +
                            row.Id +
                            '" class="btn btn-default btn-xs"  title="' +
                            LSys("SetTenantFeatures") +
                            '"  ><i class="fa fa-bars"></i></a> ';
                    } else {
                        setFeaBtn = '<a class="btn btn-default btn-xs disabled"  title="' + LSys("SetTenantFeatures") + '"  ><i class="fa fa-bars"></i></a> ';

                    }
                   
                    statsBtn = '<a href="/SysAdmin/Tenants/TenantCharts?tenantId=' +
                        row.Id +
                        '" class="btn btn-default btn-xs"><i class="fa fa-tachometer"></i></a> ';

                    delBtn = '<a data-id=' + row.Id + ' class="btn btn-danger btn-xs js-delete-tenant" title="' + LSys("DeleteTenant") + '"><i class="fa fa-times"></i></a> ';

                    return editionBtn + setFeaBtn + statsBtn + delBtn;
                },
                targets: 0
            }
        ];
        var dataTablesConfig = new DatatablesConfig({
            Url: "/SysAdmin/Tenants/" + "GetTenantsTable",
            DisplayLength: 10,
            OnInitComplete: function () {
            },
            Element: $("#tenantsTable"),
            Columns: columns,
            ColumnDefinitions: columnDefinitions
        });

        var tenantsPage = {
            dataTablesConfig: dataTablesConfig
        }

        var $table = $("#tenantsTable");

        var table = $table
        .DataTable(tenantsPage.dataTablesConfig);









    });


})();