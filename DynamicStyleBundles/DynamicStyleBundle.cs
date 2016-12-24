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
        public DynamicStyleBundle(string virtualPath, params IBundleTransform[] transforms)
            : this(virtualPath, new DefaultCacheKeyGenerator(), new DefaultCacheToggleProvider(), transforms)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicStyleBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="cdnPath">The cdn path.</param>
        /// <param name="transforms">The bundle transformers.</param>
        public DynamicStyleBundle(string virtualPath, string cdnPath, params IBundleTransform[] transforms)
            : this(virtualPath, cdnPath, new DefaultCacheKeyGenerator(), new DefaultCacheToggleProvider(), transforms)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicStyleBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="cacheKeyGenerator">The implementation of cache key generator.</param>
        /// <param name="cacheToggleProvider">The implementation of cache toggle provider.</param>
        /// <param name="transforms">The bundle transformers.</param>
        public DynamicStyleBundle(string virtualPath, ICacheKeyGenerator cacheKeyGenerator, ICacheToggleProvider cacheToggleProvider, params IBundleTransform[] transforms)
            : base(virtualPath, transforms)
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
        /// <param name="transforms">The bundle transformers.</param>
        public DynamicStyleBundle(string virtualPath, string cdnPath, ICacheKeyGenerator cacheKeyGenerator, ICacheToggleProvider cacheToggleProvider, params IBundleTransform[] transforms)
            : base(virtualPath, cdnPath, transforms)
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
            if (!_cacheToggleProvider.IsCacheEnabled)
            {
                return;
            }
            base.UpdateCache(context, response);
        }

        /// <inheritdoc />
        public override BundleResponse CacheLookup(BundleContext context)
        {
            if (!_cacheToggleProvider.IsCacheEnabled)
            {
                return null;
            }
            return base.CacheLookup(context);
        }
    }
}
