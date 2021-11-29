using System.Web;
using System.Web.Optimization;

namespace startupbuddy01
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/DataTables/datatables.min.js",
                    "~/Scripts/select2/js/select2.full.min.js",
                    "~/Scripts/sweetalert2/sweetalert2.min.js",
                    "~/Scripts/toastr/toastr.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Scripts/DataTables/datatables.min.css",
                    "~/Scripts/toastr/toastr.min.css",
                    "~/Scripts/select2/css/select2.min.css",
                    "~/Scripts/select2-bootstrap4-theme/select2-bootstrap4.min.css",
                    "~/Scripts/sweetalert2/sweetalert2.css",
                    "~/Content/site.css"));
        }
    }
}
