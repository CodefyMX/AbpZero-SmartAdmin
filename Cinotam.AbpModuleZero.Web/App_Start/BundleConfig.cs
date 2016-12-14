using System.Web.Optimization;

namespace Cinotam.AbpModuleZero.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            //VENDOR RESOURCES
            bundles.Add(new StyleBundle("~/content/smartadmin")
                    .Include("~/Areas/SysAdmin/content/css/bootstrap.min.css")
                    .Include("~/Areas/SysAdmin/content/css/font-awesome.min.css")
                    .Include("~/Areas/SysAdmin/content/css/invoice.min.css")
                    .Include("~/Areas/SysAdmin/content/css/lockscreen.min.css")
                    .Include("~/Areas/SysAdmin/content/css/smartadmin-production-plugins.min.css")
                    .Include("~/Areas/SysAdmin/content/css/smartadmin-production.min.css")
                    .Include("~/Areas/SysAdmin/content/css/smartadmin-rtl.min.css")
                    .Include("~/Areas/SysAdmin/content/css/smartadmin-skins.min.css")
                    .Include("~/Areas/SysAdmin/content/css/your_style.min.css")
                    .Include("~/Areas/SysAdmin/Content/css/abp/toastr.min.css")
                    .Include("~/Scripts/sweetalert/sweet-alert.css")
                    .Include("~/Content/flags/famfamfam-flags.css"));
            //~/Bundles/vendor/css
            bundles.Add(
                new StyleBundle("~/Bundles/vendor/css")
                    .Include("~/Content/themes/base/all.css", new CssRewriteUrlTransform())
                    .Include("~/Content/bootstrap-cosmo.min.css", new CssRewriteUrlTransform())
                    .Include("~/Content/toastr.min.css", new CssRewriteUrlTransform())
                    .Include("~/scripts/sweetalert/sweet-alert.css", new CssRewriteUrlTransform())
                    .Include("~/Content/flags/famfamfam-flags.css", new CssRewriteUrlTransform())
                    .Include("~/Content/font-awesome.min.css", new CssRewriteUrlTransform())
                );

            //~/Bundles/vendor/js/top (These scripts should be included in the head of the page)
            bundles.Add(
                new ScriptBundle("~/Bundles/vendor/js/top")
                    .Include(
                        "~/Abp/Framework/scripts/utils/ie10fix.js",
                        "~/scripts/modernizr-2.8.3.js"
                    )
                );

            //~/Bundles/vendor/bottom (Included in the bottom for fast page load)
            bundles.Add(
                new ScriptBundle("~/Bundles/vendor/js/bottom")
                    .Include(
                        "~/scripts/json2.min.js",

                        //"~/scripts/jquery-2.2.0.min.js",
                        //"~/scripts/jquery-ui-1.11.4.min.js",

                        //"~/scripts/bootstrap.min.js",

                        "~/scripts/moment-with-locales.min.js",
                        "~/scripts/jquery.validate.min.js",
                        "~/scripts/jquery.blockUI.js",
                        "~/scripts/toastr.min.js",
                        "~/scripts/sweetalert/sweet-alert.min.js",
                        "~/scripts/others/spinjs/spin.js",
                        "~/scripts/others/spinjs/jquery.spin.js",

                        "~/Abp/Framework/scripts/abp.js",
                        "~/Abp/Framework/scripts/libs/abp.jquery.js",
                        "~/Abp/Framework/scripts/libs/abp.toastr.js",
                        "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                        "~/Abp/Framework/scripts/libs/abp.spin.js",
                        "~/Abp/Framework/scripts/libs/abp.sweet-alert.js",

                        "~/scripts/jquery.signalR-2.2.1.min.js"
                    )
                );

            //APPLICATION RESOURCES

            //~/Bundles/css
            bundles.Add(
                new StyleBundle("~/Bundles/css")
                    .Include("~/css/main.css")
                );

            //~/Bundles/js
            bundles.Add(
                new ScriptBundle("~/Bundles/js")
                    .Include("~/js/main.js", "~/js/GlobalModal.js")
                );

            bundles.Add(new StyleBundle("~/content/smartadmin").IncludeDirectory("~/areas/sysadmin/content/css", "*.min.css"));

            bundles.Add(new ScriptBundle("~/scripts/smartadmin").Include(
                "~/areas/sysadmin/scripts/smartAdminScripts/app.config.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/jquery-touch/jquery.ui.touch-punch.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/bootstrap/bootstrap.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/smartwidgets/jarvis.widget.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/jquery-validate/jquery.validate.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/masked-input/jquery.maskedinput.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/select2/select2.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/bootstrap-slider/bootstrap-slider.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/bootstrap-progressbar/bootstrap-progressbar.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/msie-fix/jquery.mb.browser.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/fastclick/fastclick.min.js",
                 "~/areas/sysadmin/scripts/smartAdminScripts/notification/SmartNotification.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/app.js"));

            bundles.Add(new ScriptBundle("~/scripts/charts").Include(
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/flot/jquery.flot.cust.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/flot/jquery.flot.resize.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/flot/jquery.flot.time.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/flot/jquery.flot.fillbetween.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/flot/jquery.flot.orderBar.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/flot/jquery.flot.pie.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/flot/jquery.flot.tooltip.min.js",
                 "~/areas/sysadmin/scripts/smartAdminScripts/plugin/flot/jquery.flot.navigate.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/highcharts").Include(
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/highChartCore/highcharts-custom.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/highchartTable/jquery.highchartTable.min.js"
                ));
            bundles.Add(new ScriptBundle("~/scripts/morrischarts").Include(
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/morris/morris.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/morris/raphael.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/chartsjs").Include(
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/chartjs/chart.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/datatables").Include(
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/datatables/jquery.dataTables.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/datatables/dataTables.colVis.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/datatables/dataTables.tableTools.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/datatables/dataTables.bootstrap.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/datatable-responsive/datatables.responsive.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/jq-grid").Include(
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/jqgrid/jquery.jqGrid.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/jqgrid/grid.locale-en.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/forms").Include(
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/jquery-form/jquery-form.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/smart-chat").Include(
                "~/areas/sysadmin/scripts/smartAdminScripts/smart-chat-ui/smart.chat.ui.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/smart-chat-ui/smart.chat.manager.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/vector-map").Include(
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/vectormap/jquery-jvectormap-1.2.2.min.js",
                "~/areas/sysadmin/scripts/smartAdminScripts/plugin/vectormap/jquery-jvectormap-world-mill-en.js"
                ));







            #region SpaResources
            bundles.Add(
               new ScriptBundle("~/Bundles/App/vendor/js")
                   .Include(
                       "~/Abp/Framework/scripts/utils/ie10fix.js",
                       "~/Scripts/json2.min.js",

                       "~/Scripts/modernizr-2.8.3.js",

                       "~/Scripts/jquery-2.2.0.min.js",
                       "~/Scripts/jquery-ui-1.11.4.min.js",

                       "~/Scripts/bootstrap.min.js",

                       "~/Scripts/moment-with-locales.min.js",
                       "~/Scripts/jquery.validate.min.js",
                       "~/Scripts/jquery.blockUI.js",
                       "~/Scripts/toastr.min.js",
                       "~/Scripts/sweetalert/sweet-alert.min.js",
                       "~/Scripts/others/spinjs/spin.js",
                       "~/Scripts/others/spinjs/jquery.spin.js",

                       "~/Scripts/angular.min.js",
                       "~/Scripts/angular-animate.min.js",
                       "~/Scripts/angular-sanitize.min.js",
                       "~/Scripts/angular-ui-router.min.js",
                       "~/Scripts/angular-ui/ui-bootstrap.min.js",
                       "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
                       "~/Scripts/angular-ui/ui-utils.min.js",

                       "~/Abp/Framework/scripts/abp.js",
                       "~/Abp/Framework/scripts/libs/abp.jquery.js",
                       "~/Abp/Framework/scripts/libs/abp.toastr.js",
                       "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                       "~/Abp/Framework/scripts/libs/abp.spin.js",
                       "~/Abp/Framework/scripts/libs/abp.sweet-alert.js",
                       "~/Abp/Framework/scripts/libs/angularjs/abp.ng.js",

                       "~/Scripts/jquery.signalR-2.2.1.min.js"
                   )
               );

            bundles.Add(
                new ScriptBundle("~/App/SysAdmin/js")
                    .IncludeDirectory("~/Common/Scripts", "*.js", true)
                    .IncludeDirectory("~/App/SysAdmin/Main", "*.js", true)
                );


            bundles.Add(new ScriptBundle("~/App/SysAdmin/Libs/js")
                .Include("~/App/dependencies/angular-chart.js/chart.min.js")
                .Include("~/Scripts/jstree/jstree.min.js")
                .Include("~/Scripts/jstree/ngJstree.min.js")
                .Include("~/App/dependencies/ui-bootstrap/ui-bootstrap-tpls-2.3.0.min.js")
                .Include("~/areas/sysadmin/scripts/smartAdminScripts/plugin/datatables/jquery.dataTables.min.js")

                .IncludeDirectory("~/App/dependencies", "*.js", true)

                );




            bundles.Add(new ScriptBundle("~/App/SysAdmin/Blocks/js")
                .Include("~/App/Blocks/logger/logger.module.js",
                "~/App/Blocks/logger/logger.js",
                "~/App/Blocks/router/router.module.js",
                "~/App/Blocks/router/router.helper.provider.js")
                .IncludeDirectory("~/App/Core", "*.js", true));
            #endregion





            BundleTable.EnableOptimizations = false;
        }
    }
}