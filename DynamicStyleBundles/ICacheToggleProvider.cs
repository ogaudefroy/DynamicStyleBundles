namespace DynamicStyleBundles
{
    /// <summary>
    /// Provides a feature toggle aiming at enabling / disabling stylebundle caching.
    /// </summary>
    public interface ICacheToggleProvider
    {
        /// <summary>
        /// Gets a value indicating whether or not the caching is active for the current request.
        /// </summary>
        /// <returns>True if caching is active, False otherwise.</returns>
        bool IsCacheEnabled { get; }
    }
}
