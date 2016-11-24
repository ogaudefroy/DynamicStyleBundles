namespace DynamicStyleBundles
{
    /// <summary>
    /// Default cache toggle provider.
    /// Caching is always active.
    /// </summary>
    public class DefaultCacheToggleProvider : ICacheToggleProvider
    {
        /// <inheritdoc />
        public bool IsCacheEnabled
        {
            get
            {
                return true;
            }
        }
    }
}
