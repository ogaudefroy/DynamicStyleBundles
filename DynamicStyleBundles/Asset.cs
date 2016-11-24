namespace DynamicStyleBundles
{
    using System;
    using System.Text;

    /// <summary>
    /// Data structure holding the asset itself.
    /// </summary>
    public class Asset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Asset"/> class.
        /// </summary>
        /// <param name="data">The raw data.</param>
        /// <param name="lastWriteTime">The last time the asset was modified.</param>
        public Asset(byte[] data, DateTime lastWriteTime)
        {
            this.Data = data;
            this.LastWriteTime = lastWriteTime;
        }

        /// <summary>
        /// Gets the raw data of the content.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Gets the last write time of the content.
        /// </summary>
        public DateTime LastWriteTime { get; private set; }

        /// <summary>
        /// Gets the content as text
        /// </summary>
        public string TextContent
        {
            get
            {
                return Encoding.UTF8.GetString(this.Data);
            }
        }
    }
}