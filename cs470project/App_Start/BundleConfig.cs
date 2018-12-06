﻿using System.Web;
using System.Web.Optimization;

namespace cs470project
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootbox.js",
                        "~/Scripts/toastr.js",
                        "~/Scripts/typeahead.bundle.js",
                        "~/Scripts/DataTables/jquery.dataTables.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/DataTables/dataTables.bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-cyborg.css",
                      "~/Content/DataTables/css/dataTables.bootstrap.css",
                      "~/Content/toastr.css",
                      "~/Content/typeahead.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/dashboard_index").Include(
                      "~/Content/Scripts/Dashboard/Index.js"));

            bundles.Add(new ScriptBundle("~/bundles/ProjectDashboard").Include(
                      "~/Content/Scripts/Dashboard/ProjectDashboard.js",
					  "~/Content/Scripts/Dashboard/Upload.js",
                      "~/Content/Scripts/Dashboard/Users.js"));

            bundles.Add(new ScriptBundle("~/bundles/NewProject").Include(
                      "~/Content/Scripts/Dashboard/NewProject.js"));

            bundles.Add(new ScriptBundle("~/bundles/EditProject").Include(
                      "~/Content/Scripts/Dashboard/EditProject.js"));
        }
    }
}
