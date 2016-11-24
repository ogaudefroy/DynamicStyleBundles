namespace DynamicStyleBundles
{
    using System;
    using System.IO;
    using System.Web.Hosting;

    /// <summary>
    /// The virtual file containing the asset.
    /// </summary>
    public class CustomVirtualFile : VirtualFile
    {
        private readonly Asset _asset;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomVirtualFile"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="asset">The asset itself.</param>
        public CustomVirtualFile(string virtualPath, Asset asset)
            : base(virtualPath)
        {
            if (asset == null)
            {
                throw new ArgumentNullException("asset");
            }
            _asset = asset;
        }

        /// <inheritdoc />
        public override Stream Open()
        {
            return new MemoryStream(_asset.Data);
        }
    }
}
