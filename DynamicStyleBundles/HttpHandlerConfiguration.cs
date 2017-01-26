namespace DynamicStyleBundles
{
    using System;

    /// <summary>
    /// Configuration class for the HttpHandler.
    /// </summary>
    public class HttpHandlerConfiguration
    {
        /// <summary>
        /// Gets or sets the current handler configuration.
        /// </summary>
        public static HttpHandlerConfiguration Current
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHandlerConfiguration"/> class.
        /// </summary>
        /// <param name="assetLoaderFuncter">A functer to retrieve the asset loader.</param>
        /// <param name="virtualPath">The virtual path under which the handler is mapped.</param>
        public HttpHandlerConfiguration(Func<IAssetLoader> assetLoaderFuncter, string virtualPath)
        {
            if (assetLoaderFuncter == null)
            {
                throw new ArgumentNullException("assetLoaderFuncter");
            }
            if (virtualPath == null)
            {
                throw new ArgumentNullException("virtualPath");
            }
            this.AssetLoaderFuncter = assetLoaderFuncter;
            this.VirtualPath = virtualPath;
        }

        /// <summary>
        /// Gets a reference to the asset loader functer.
        /// </summary>
        public Func<IAssetLoader> AssetLoaderFuncter
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the mapped virtual path of the handler.
        /// </summary>
        public string VirtualPath
        {
            get;
            private set;
        }
    }
}
