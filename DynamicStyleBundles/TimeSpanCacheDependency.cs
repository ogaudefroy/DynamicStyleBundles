namespace DynamicStyleBundles
{
    using System;
    using System.Threading;
    using System.Web.Caching;

    /// <summary>
    /// Cache dependency implementing only absolute expiration support.
    /// </summary>
    public class TimeSpanCacheDependency : CacheDependency
    {
        private readonly Timer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanCacheDependency"/> class.
        /// </summary>
        /// <param name="absoluteExpiration">The number of seconds before absolute expiration.</param>
        public TimeSpanCacheDependency(int absoluteExpiration)
        {
            if (absoluteExpiration < 1)
            {
                throw new ArgumentException("absoluteExpiration");
            }
            _timer = new Timer((sender) => this.NotifyDependencyChanged(sender, EventArgs.Empty), this, TimeSpan.FromSeconds(absoluteExpiration), TimeSpan.FromSeconds(absoluteExpiration));
        }

        /// <inheritdoc />
        protected override void DependencyDispose()
        {
            if (this._timer != null)
            {
                _timer.Dispose();
            }
            base.DependencyDispose();
        }
    }
}
