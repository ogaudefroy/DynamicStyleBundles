namespace DynamicStyleBundles.Tests
{
    using NUnit.Framework;
    
    [TestFixture]
    public class DefaultCacheToggleProviderTest
    {
        [Test]
        public void DefaultCacheToggleProvider_Returns_True()
        {
            var provider = new DefaultCacheToggleProvider();
            Assert.That(provider.IsCacheEnabled, Is.True);
        }
    }
}
