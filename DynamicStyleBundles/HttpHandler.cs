namespace DynamicStyleBundles
{
    using System;
    using System.IO;
    using System.Net;
    using System.Web;
    using System.Web.Routing;
    
    /// <summary>
    /// HTTP handler used to retrieve dynamically customer assets.
    /// </summary>
    public class HttpHandler : IHttpHandler, IRouteHandler
    {
        private readonly string _virtualDirectoryName;
        private static Func<IAssetLoader> _assetLoaderLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHandler"/> class using the current registered configuration.
        /// </summary>
        public HttpHandler()
            : this(HttpHandlerConfiguration.Current)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHandler"/> class using the provided configuration.
        /// </summary>
        /// <param name="config">The provided HTTP handler configuration.</param>
        public HttpHandler(HttpHandlerConfiguration config) 
            : this(config.AssetLoaderFuncter, config.VirtualPath)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHandler"/> class.
        /// </summary>
        /// <param name="assetLoaderLocator">Function to retrieve the content</param>
        /// <param name="virtualDirectoryName">The name of the dynamic content folder</param>
        public HttpHandler(Func<IAssetLoader> assetLoaderLocator, string virtualDirectoryName)
        {
            if (assetLoaderLocator == null)
            {
                throw new ArgumentNullException("assetLoaderLocator");
            }
            if (string.IsNullOrEmpty(virtualDirectoryName))
            {
                throw new ArgumentNullException("virtualDirectoryName");
            }
            _virtualDirectoryName = virtualDirectoryName;
            _assetLoaderLocator = assetLoaderLocator;
        }

        /// <summary>
        /// Gets a value indicating whether the handler is reusable.
        /// </summary>
        /// <remarks>Returns True as the handler has a dependency on a service locator used to retrieve the content.</remarks>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Process the HTTP request, public endpoint.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        public void ProcessRequest(HttpContext context)
        {
            this.ProcessRequestInternal(new HttpContextWrapper(context));
        }

        /// <summary>
        /// Internal endpoint with testing capabilities.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        internal void ProcessRequestInternal(HttpContextBase context)
        {
            var resourceParam = context.Request.QueryString["resource"];

            var apprelativePath = VirtualPathUtility.AppendTrailingSlash(context.Request.ApplicationPath) + this._virtualDirectoryName;

            var filePath = !string.IsNullOrEmpty(resourceParam) ? resourceParam : context.Request.Path.Substring(context.Request.Path.IndexOf(apprelativePath, StringComparison.CurrentCultureIgnoreCase) + apprelativePath.Length);

            var assetLoader = _assetLoaderLocator();
            if (assetLoader == null)
            {
                throw new NotSupportedException("Unable to get a not null instance of IContentRetriever.");
            }

            var asset = assetLoader.Load(filePath);
            if (asset == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }
            var fileName = Path.GetFileName(filePath);
            context.Response.ContentType = MimeMapping.GetMimeMapping(fileName);

            var responseContent = asset.Data;
            var refresh = new TimeSpan(0, 1, 0, 0);

            context.Response.Cache.SetExpires(DateTime.Now.Add(refresh));
            context.Response.Cache.SetMaxAge(refresh);
            context.Response.Cache.SetLastModified(asset.LastWriteTime);
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.CacheControl = HttpCacheability.Public.ToString();
            context.Response.Cache.SetValidUntilExpires(true);
            context.Response.Cache.VaryByParams["random"] = true;
            context.Response.Cache.VaryByParams["resource"] = true;
            context.Response.Cache.VaryByHeaders["Host"] = true;
            context.Response.BinaryWrite(responseContent);
        }

        /// <summary>
        /// Returns the HTTP handler bound for this route (ie. itself).
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <returns>The <see cref="IHttpHandler"/>.</returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }
    }
}