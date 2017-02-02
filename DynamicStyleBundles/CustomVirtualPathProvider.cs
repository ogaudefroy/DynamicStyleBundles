namespace DynamicStyleBundles
{
    using System;
    using System.Collections;
    using System.Web.Caching;
    using System.Web.Hosting;

    /// <summary>
    /// Custom virtual path provider handling specific directory containing dynamic assets.
    /// If virtualPath starts with dynamicAssetsDirectory value then asset is loaded and returned otherwise previous virtual path provider is used.
    /// </summary>
    public class CustomVirtualPathProvider : VirtualPathProvider
    {
        private readonly VirtualPathProvider _previous;
        private readonly string _dynamicAssetsVirtualPath;
        private readonly Func<IAssetLoader> _assetLoaderLocator;
        private readonly ICacheDependencyBuilder _cacheDependencyBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomVirtualPathProvider"/> class.
        /// </summary>
        /// <param name="previous">The previous virtual path provider.</param>
        /// <param name="dynamicAssetsDirectory">The folder path containing dynamic assets.</param>
        /// <param name="assetLoaderLocator">The content retriever service locator.</param>
        /// <remarks>Cache dependency builder used will be DefaultCacheDependencyBuilder.</remarks>
        public CustomVirtualPathProvider(VirtualPathProvider previous, string dynamicAssetsDirectory, Func<IAssetLoader> assetLoaderLocator)
            : this(previous, dynamicAssetsDirectory, assetLoaderLocator, new DefaultCacheDependencyBuilder())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomVirtualPathProvider"/> class.
        /// </summary>
        /// <param name="previous">The previous virtual path provider.</param>
        /// <param name="dynamicAssetsVirtualPath">The virtual path containing dynamic assets.</param>
        /// <param name="assetLoaderLocator">The content retriever service locator.</param>
        /// <param name="cacheDependencyBuilder">A builder used to generate cache dependencies.</param>
        public CustomVirtualPathProvider(VirtualPathProvider previous, string dynamicAssetsVirtualPath, Func<IAssetLoader> assetLoaderLocator, ICacheDependencyBuilder cacheDependencyBuilder)
        {
            if (previous == null)
            {
                throw new ArgumentNullException("previous");
            }
            if (string.IsNullOrEmpty(dynamicAssetsVirtualPath))
            {
                throw new ArgumentNullException("dynamicAssetsVirtualPath");
            }
            if (assetLoaderLocator == null)
            {
                throw new ArgumentNullException("assetLoaderLocator");
            }
            if (cacheDependencyBuilder == null)
            {
                throw new ArgumentNullException("cacheDependencyBuilder");
            }
            _previous = previous;
            _dynamicAssetsVirtualPath = dynamicAssetsVirtualPath;
            _assetLoaderLocator = assetLoaderLocator;
            _cacheDependencyBuilder = cacheDependencyBuilder;
        }

        /// <inheritdoc />
        public override bool FileExists(string virtualPath)
        {
            if (this.IsEmbeddedPath(virtualPath))
            {
                return true;
            }
            return _previous.FileExists(virtualPath);
        }

        /// <inheritdoc />
        public override CacheDependency GetCacheDependency(
            string virtualPath,
            IEnumerable virtualPathDependencies,
            DateTime utcStart)
        {
            if (this.IsEmbeddedPath(virtualPath))
            {
                return _cacheDependencyBuilder.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
            }
            return _previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        /// <inheritdoc />
        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            return _previous.GetDirectory(virtualDir);
        }

        /// <inheritdoc />
        public override bool DirectoryExists(string virtualDir)
        {
            return _previous.DirectoryExists(virtualDir);
        }

        /// <inheritdoc />
        public override VirtualFile GetFile(string virtualPath)
        {
            if (this.IsEmbeddedPath(virtualPath))
            {
                var assetLoader = _assetLoaderLocator();
                var filePath = virtualPath.Substring(virtualPath.IndexOf(_dynamicAssetsVirtualPath, StringComparison.CurrentCultureIgnoreCase) + _dynamicAssetsVirtualPath.Length);
                var content = assetLoader.Load(filePath);
                return new CustomVirtualFile(virtualPath, content);
            }
            return _previous.GetFile(virtualPath);
        }

        /// <summary>
        /// Return true if the path is a dynamic one.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool IsEmbeddedPath(string path)
        {
            return path.StartsWith(_dynamicAssetsVirtualPath, StringComparison.OrdinalIgnoreCase);
        }
    }
}