using System.Web;
using System.Web.Optimization;

namespace PollSystem
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.9.1.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/move-top.js",
                        "~/Scripts/easing.js",
                        "~/Scripts/wow.min.js",
                        "~/Scripts/i18n/grid.locale-en.js",
                        "~/Scripts/jquery.jqGrid.min.js"));
           
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/style.css",
                      "~/Content/font-awesome.css",
                       "~/Content/animate.css",
                       "~/Content/jquery.jqGrid/ui.jqgrid.css",
                       "~/Content/themes/base/jquery-ui.css",
                       "~/Content/themes/base/jquery.ui.all.css"));
        }
    }
}