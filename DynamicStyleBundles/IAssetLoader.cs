namespace DynamicStyleBundles
{

    /// <summary>
    /// Interface describing the contract to implement in order to retrieve dynamically an asset from an URI segment (/picto/check.gif).
    /// </summary>
    public interface IAssetLoader
    {
        /// <summary>
        /// Returns the asset if exists, null otherwise.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The asset loaded from the store.</returns>
        Asset Load(string filePath);
    }
}
