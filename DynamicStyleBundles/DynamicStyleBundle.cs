namespace DynamicStyleBundles
{
    using System;
    using System.Web.Optimization;

    /// <summary>
    /// Custom Style bundle supporting multi-tenancy (per host cache key) and cache disabling (?nocsscache=1).
    /// </summary>
    public class DynamicStyleBundle : Bundle
    {
        private readonly ICacheKeyGenerator _cacheKeyGenerator;
        private readonly ICacheToggleProvider _cacheToggleProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicStyleBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        public DynamicStyleBundle(string virtualPath)
            : this(virtualPath, new DefaultCacheKeyGenerator(), new DefaultCacheToggleProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicStyleBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="cdnPath">The cdn path.</param>
        public DynamicStyleBundle(string virtualPath, string cdnPath)
            : this(virtualPath, cdnPath, new DefaultCacheKeyGenerator(), new DefaultCacheToggleProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicStyleBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="cacheKeyGenerator">The implementation of cache key generator.</param>
        /// <param name="cacheToggleProvider">The implementation of cache toggle provider.</param>
        public DynamicStyleBundle(string virtualPath, ICacheKeyGenerator cacheKeyGenerator, ICacheToggleProvider cacheToggleProvider)
            : base(virtualPath)
        {
            if (cacheKeyGenerator == null)
            {
                throw new ArgumentNullException("cacheKeyGenerator");
            }
            if (cacheToggleProvider == null)
            {
                throw new ArgumentNullException("cacheToggleProvider");
            }
            _cacheKeyGenerator = cacheKeyGenerator;
            _cacheToggleProvider = cacheToggleProvider;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicStyleBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="cdnPath">The cdn path.</param>
        /// <param name="cacheKeyGenerator">The implementation of cache key generator.</param>
        /// <param name="cacheToggleProvider">The implementation of cache toggle provider.</param>
        public DynamicStyleBundle(string virtualPath, string cdnPath, ICacheKeyGenerator cacheKeyGenerator, ICacheToggleProvider cacheToggleProvider)
            : base(virtualPath, cdnPath)
        {
            if (cacheKeyGenerator == null)
            {
                throw new ArgumentNullException("cacheKeyGenerator");
            }
            if (cacheToggleProvider == null)
            {
                throw new ArgumentNullException("cacheToggleProvider");
            }
            _cacheKeyGenerator = cacheKeyGenerator;
            _cacheToggleProvider = cacheToggleProvider;
        }

        /// <inheritdoc />
        public override string GetCacheKey(BundleContext context)
        {
            return _cacheKeyGenerator.GetCacheKey(base.GetCacheKey(context));
        }

        /// <inheritdoc />
        public override void UpdateCache(BundleContext context, BundleResponse response)
        {
            if (!this._cacheToggleProvider.IsCacheEnabled)
            {
                return;
            }
            base.UpdateCache(context, response);
        }

        /// <inheritdoc />
        public override BundleResponse CacheLookup(BundleContext context)
        {
            if (!this._cacheToggleProvider.IsCacheEnabled)
            {
                return null;
            }
            return base.CacheLookup(context);
        }
    }
}
