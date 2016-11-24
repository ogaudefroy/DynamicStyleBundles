namespace DynamicStyleBundles
{
    /// <summary>
    /// Provides a tenant name.
    /// </summary>
    public interface ICacheKeyGenerator
    {
        /// <summary>
        /// Generates a unique caching key.
        /// </summary>
        /// <param name="originalKey">The original key emitted by the style bundle.</param>
        /// <returns>Unique key.</returns>
        string GetCacheKey(string originalKey);
    }
}
