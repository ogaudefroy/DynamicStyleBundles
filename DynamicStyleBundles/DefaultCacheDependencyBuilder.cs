namespace DynamicStyleBundles
{
    using System;
    using System.Collections;
    using System.Web.Caching;

    /// <summary>
    /// Default cache dependency builder implementing a timespan cache dependency with 15 minutes caching.
    /// </summary>
    public class DefaultCacheDependencyBuilder : ICacheDependencyBuilder
    {
        private readonly int _absoluteExpiration = 900;

        /// <inheritdoc />
        public CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return new TimeSpanCacheDependency(_absoluteExpiration);
        }
    }
}
