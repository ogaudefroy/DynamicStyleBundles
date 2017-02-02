namespace DynamicStyleBundles.Sample
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            DynamicStyleBundlesConfig.Current = new DynamicStyleBundlesConfig(() => GetAssetLoader(), "~/dynamic");
            DynamicStyleBundlesConfig.Current.ApplyConfig();

            BundleTable.EnableOptimizations = true;
            BundleTable.Bundles.Add(new DynamicStyleBundle("~/dynamic/style").Include("~/default.css"));
            RouteTable.Routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        private static IAssetLoader GetAssetLoader()
        {
            return new TestAssetLoader(new HttpContextWrapper(HttpContext.Current));
        }

        class TestAssetLoader : IAssetLoader
        {
            private readonly HttpContextBase _ctx;

            public TestAssetLoader(HttpContextBase ctx)
            {
                _ctx = ctx;
            }

            public Asset Load(string filePath)
            {
                var computedPath = _ctx.Request.IsSecureConnection ? _ctx.Server.MapPath("~/Assets/Https" + filePath) : _ctx.Server.MapPath("~/Assets/Http" + filePath);
                var fileInfo = new FileInfo(computedPath);
                return new Asset(File.ReadAllBytes(filePath), fileInfo.LastWriteTimeUtc);
            }
        }
    }
}