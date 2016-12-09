using System.Web;
using System.Web.Optimization;

namespace Budgeter
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Content/metisMenu/metisMenu.min.js",
                      "~/Content/raphael/raphael.min.js",
                      "~/Content/morrisjs/morris.min.js",
                      "~/Content/morris-data.js",
                      "~/Scripts/sb-admin-2.js"));

            

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/metisMenu/metisMenu.min.css",
                      "~/Content/sb-admin-2.css",
                      "~/Content/morrisjs/morris.css",
                      "~/Content/font-awesome/css/font-awesome.min.css"));
        }
    }
}
