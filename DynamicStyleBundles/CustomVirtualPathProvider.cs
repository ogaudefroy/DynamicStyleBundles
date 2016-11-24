namespace DynamicStyleBundles
{
    using System;
    using System.Collections;
    using System.Web.Caching;
    using System.Web.Hosting;

    /// <summary>
    /// Custom virtual path provider which handles specific directory containing DynamicContent.
    /// If virtualPath starts with dynamicContentDirectory then Content is returned otherwise previous VirtualPathProvider is used.
    /// </summary>
    public class CustomVirtualPathProvider : VirtualPathProvider
    {
        private readonly VirtualPathProvider _previous;
        private readonly string _dynamicContentDirectory;
        private readonly Func<IAssetLoader> _assetLoaderLocator;
        private readonly int _cacheAbsoluteExpiration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentVirtualPathProvider"/> class.
        /// </summary>
        /// <param name="previous">The previous virtual path provider.</param>
        /// <param name="dynamicContentDirectory">The folder path containing dynamic content.</param>
        /// <param name="contentRetrieverLocator">The content retriever service locator.</param>
        /// <param name="cacheAbsoluteExpiration">The cache absolute expiration.</param>
        public CustomVirtualPathProvider(VirtualPathProvider previous, string dynamicContentDirectory, Func<IAssetLoader> assetLoaderLocator, int cacheAbsoluteExpiration)
        {
            if (previous == null)
            {
                throw new ArgumentNullException("previous");
            }
            if (string.IsNullOrEmpty(dynamicContentDirectory))
            {
                throw new ArgumentNullException("dynamicContentDirectory");
            }
            if (assetLoaderLocator == null)
            {
                throw new ArgumentNullException("assetLoaderLocator");
            }
            if (cacheAbsoluteExpiration < 1)
            {
                throw new ArgumentException("Cache expiration must be a postive integer.", "cacheAbsoluteExpiration");
            }
            _previous = previous;
            _dynamicContentDirectory = string.Format("~/{0}", dynamicContentDirectory);
            _assetLoaderLocator = assetLoaderLocator;
            _cacheAbsoluteExpiration = cacheAbsoluteExpiration;
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
                return new TimeSpanCacheDependency(_cacheAbsoluteExpiration);
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
                var filePath = virtualPath.Substring(virtualPath.IndexOf(_dynamicContentDirectory, StringComparison.CurrentCultureIgnoreCase) + _dynamicContentDirectory.Length);
                var content = assetLoader.Load(filePath);
                return new CustomVirtualFile(virtualPath, content);
            }
            return this._previous.GetFile(virtualPath);
        }

        /// <summary>
        /// Return true if the path is a dynamic one.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool IsEmbeddedPath(string path)
        {
            return path.StartsWith(this._dynamicContentDirectory, StringComparison.OrdinalIgnoreCase);
        }
    }
}