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
                    .Include("~/content/css/bootstrap.min.css")
                    .Include("~/content/css/demo.min.css")
                    .Include("~/content/css/font-awesome.min.css")
                    .Include("~/content/css/invoice.min.css")
                    .Include("~/content/css/lockscreen.min.css")
                    .Include("~/content/css/smartadmin-production-plugins.min.css")
                    .Include("~/content/css/smartadmin-production.min.css")
                    .Include("~/content/css/smartadmin-rtl.min.css")
                    .Include("~/content/css/smartadmin-skins.min.css")
                    .Include("~/content/css/your_style.min.css")
                    .Include("~/content/abp/toastr.min.css", new CssRewriteUrlTransform())
                    .Include("~/scripts/sweetalert/sweet-alert.css", new CssRewriteUrlTransform())
                    .Include("~/Content/flags/famfamfam-flags.css", new CssRewriteUrlTransform()));
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
                        "~/scripts//json2.min.js",

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

                        "~/scripts/jquery.signalR-2.2.0.min.js"
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

            bundles.Add(new StyleBundle("~/content/smartadmin").IncludeDirectory("~/content/css", "*.min.css"));

            bundles.Add(new ScriptBundle("~/scripts/smartadmin").Include(
                "~/scripts/smartAdminScripts/app.config.js",
                "~/scripts/smartAdminScripts/plugin/jquery-touch/jquery.ui.touch-punch.min.js",
                "~/scripts/smartAdminScripts/bootstrap/bootstrap.min.js",
                "~/scripts/smartAdminScripts/notification/SmartNotification.min.js",
                "~/scripts/smartAdminScripts/smartwidgets/jarvis.widget.min.js",
                "~/scripts/smartAdminScripts/plugin/jquery-validate/jquery.validate.min.js",
                "~/scripts/smartAdminScripts/plugin/masked-input/jquery.maskedinput.min.js",
                "~/scripts/smartAdminScripts/plugin/select2/select2.min.js",
                "~/scripts/smartAdminScripts/plugin/bootstrap-slider/bootstrap-slider.min.js",
                "~/scripts/smartAdminScripts/plugin/bootstrap-progressbar/bootstrap-progressbar.min.js",
                "~/scripts/smartAdminScripts/plugin/msie-fix/jquery.mb.browser.min.js",
                "~/scripts/smartAdminScripts/plugin/fastclick/fastclick.min.js",
                "~/scripts/smartAdminScripts/app.min.js"));

            bundles.Add(new ScriptBundle("~/scripts/full-calendar").Include(
                "~/scripts/smartAdminScripts/plugin/moment/moment.min.js",
                "~/scripts/smartAdminScripts/plugin/fullcalendar/jquery.fullcalendar.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/charts").Include(
                "~/scripts/smartAdminScripts/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js",
                "~/scripts/smartAdminScripts/plugin/sparkline/jquery.sparkline.min.js",
                "~/scripts/smartAdminScripts/plugin/morris/morris.min.js",
                "~/scripts/smartAdminScripts/plugin/morris/raphael.min.js",
                "~/scripts/smartAdminScripts/plugin/flot/jquery.flot.cust.min.js",
                "~/scripts/smartAdminScripts/plugin/flot/jquery.flot.resize.min.js",
                "~/scripts/smartAdminScripts/plugin/flot/jquery.flot.time.min.js",
                "~/scripts/smartAdminScripts/plugin/flot/jquery.flot.fillbetween.min.js",
                "~/scripts/smartAdminScripts/plugin/flot/jquery.flot.orderBar.min.js",
                "~/scripts/smartAdminScripts/plugin/flot/jquery.flot.pie.min.js",
                "~/scripts/smartAdminScripts/plugin/flot/jquery.flot.tooltip.min.js",
                "~/scripts/smartAdminScripts/plugin/dygraphs/dygraph-combined.min.js",
                "~/scripts/smartAdminScripts/plugin/chartjs/chart.min.js",
                "~/scripts/smartAdminScripts/plugin/highChartCore/highcharts-custom.min.js",
                "~/scripts/smartAdminScripts/plugin/highchartTable/jquery.highchartTable.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/datatables").Include(
                "~/scripts/smartAdminScripts/plugin/datatables/jquery.dataTables.min.js",
                "~/scripts/smartAdminScripts/plugin/datatables/dataTables.colVis.min.js",
                "~/scripts/smartAdminScripts/plugin/datatables/dataTables.tableTools.min.js",
                "~/scripts/smartAdminScripts/plugin/datatables/dataTables.bootstrap.min.js",
                "~/scripts/smartAdminScripts/plugin/datatable-responsive/datatables.responsive.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/jq-grid").Include(
                "~/scripts/smartAdminScripts/plugin/jqgrid/jquery.jqGrid.min.js",
                "~/scripts/smartAdminScripts/plugin/jqgrid/grid.locale-en.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/forms").Include(
                "~/scripts/smartAdminScripts/plugin/jquery-form/jquery-form.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/smart-chat").Include(
                "~/scripts/smartAdminScripts/smart-chat-ui/smart.chat.ui.min.js",
                "~/scripts/smartAdminScripts/smart-chat-ui/smart.chat.manager.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/vector-map").Include(
                "~/scripts/smartAdminScripts/plugin/vectormap/jquery-jvectormap-1.2.2.min.js",
                "~/scripts/smartAdminScripts/plugin/vectormap/jquery-jvectormap-world-mill-en.js"
                ));

            BundleTable.EnableOptimizations = false;
        }
    }
}