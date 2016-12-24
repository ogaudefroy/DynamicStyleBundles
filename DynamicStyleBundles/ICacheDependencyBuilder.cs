namespace DynamicStyleBundles
{
    using System;
    using System.Collections;
    using System.Web.Caching;

    /// <summary>
    /// Builds a cache dependency for a virtual path.
    /// </summary>
    public interface ICacheDependencyBuilder
    {
        /// <summary>
        /// Creates a cache dependency based on the specified virtual paths.
        /// </summary>
        /// <param name="virtualPath">The path to the primary virtual resource.</param>
        /// <param name="virtualPathDependencies">An array of paths to other resources required by the primary virtual resource.</param>
        /// <param name="utcStart">The UTC time at which the virtual resources were read.</param>
        /// <returns>A <see cref="CacheDependency" /> object for the specified virtual resources.</returns>
        CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart);
    }
}
