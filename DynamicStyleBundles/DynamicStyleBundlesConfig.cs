namespace DynamicStyleBundles
{
    using System;
    using System.Web.Optimization;

    /// <summary>
    /// Configuration class for the library.
    /// </summary>
    public class DynamicStyleBundlesConfig
    {
        /// <summary>
        /// Gets or sets the current handler configuration.
        /// </summary>
        public static DynamicStyleBundlesConfig Current
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicStyleBundlesConfig"/> class.
        /// </summary>
        /// <param name="assetLoaderFuncter">A functer to retrieve the asset loader.</param>
        /// <param name="virtualPath">The virtual path under which the handler is mapped.</param>
        public DynamicStyleBundlesConfig(Func<IAssetLoader> assetLoaderFuncter, string virtualPath)
            : this(assetLoaderFuncter, virtualPath, new DefaultCacheDependencyBuilder())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicStyleBundlesConfig"/> class.
        /// </summary>
        /// <param name="assetLoaderFuncter">A functer to retrieve the asset loader.</param>
        /// <param name="virtualPath">The virtual path under which the handler is mapped.</param>
        /// <param name="cacheDependencyBuilder"></param>
        public DynamicStyleBundlesConfig(Func<IAssetLoader> assetLoaderFuncter, string virtualPath, ICacheDependencyBuilder cacheDependencyBuilder)
        {
            if (assetLoaderFuncter == null)
            {
                throw new ArgumentNullException("assetLoaderFuncter");
            }
            if (virtualPath == null)
            {
                throw new ArgumentNullException("virtualPath");
            }
            if (cacheDependencyBuilder == null)
            {
                throw new ArgumentNullException("cacheDependencyBuilder");
            }
            this.AssetLoaderFuncter = assetLoaderFuncter;
            this.VirtualPath = virtualPath;
            this.CacheDependencyBuilder = cacheDependencyBuilder;
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

        /// <summary>
        /// Gets the associated cache dependency builder.
        /// </summary>
        public ICacheDependencyBuilder CacheDependencyBuilder
        {
            get;
            private set;
        }

        /// <summary>
        /// Applies the actual configuration on the bundle table.
        /// </summary>
        public void ApplyConfig()
        {
            BundleTable.VirtualPathProvider = new CustomVirtualPathProvider(BundleTable.VirtualPathProvider, this.VirtualPath, this.AssetLoaderFuncter, this.CacheDependencyBuilder);
        }
    }
}
