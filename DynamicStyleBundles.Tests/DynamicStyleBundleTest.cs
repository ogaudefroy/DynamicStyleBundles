namespace DynamicStyleBundles.Tests
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Optimization;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DynamicStyleBundleTest
    {
        [Test]
        public void DynamicStyleBundle_ConstructorWithNullCacheKeyGenerator_Throws()
        {
            Assert.That(
                () => new DynamicStyleBundle("~/a.txt", null, new DefaultCacheToggleProvider()),
                Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void DynamicStyleBundle_ConstructorWithNullCacheToggleProvider_Throws()
        {
            Assert.That(
                () => new DynamicStyleBundle("~/a.txt", new DefaultCacheKeyGenerator(), null),
                Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void DynamicStyleBundle_Constructor_MapsValues()
        {
            var bundle = new DynamicStyleBundle("~/common/styles", "http://mycdn.org/myApp");

            Assert.That(bundle.Path, Is.EqualTo("~/common/styles"));
            Assert.That(bundle.CdnPath, Is.EqualTo("http://mycdn.org/myApp"));
        }

        [Test]
        public void DynamicStyleBundle_GetCacheKey_ReturnsHostNamePrefixedValue()
        {
            var mockCacheKeyGenerator = new Mock<ICacheKeyGenerator>();
            mockCacheKeyGenerator.Setup(p => p.GetCacheKey("System.Web.Optimization.Bundle:/common/styles")).Returns("HOSTNAME:System.Web.Optimization.Bundle:/common/styles");

            var dynamicStyleBundle = new DynamicStyleBundle("~/common/styles", mockCacheKeyGenerator.Object, new DefaultCacheToggleProvider());
            var context = new BundleContext(new Mock<HttpContextBase>().Object, new BundleCollection(), "/common/styles");

            var key = dynamicStyleBundle.GetCacheKey(context);

            Assert.That(key, Is.EqualTo("HOSTNAME:System.Web.Optimization.Bundle:/common/styles"));
            mockCacheKeyGenerator.Verify(p => p.GetCacheKey("System.Web.Optimization.Bundle:/common/styles"), Times.Once());
        }

        [Test]
        public void DynamicStyleBundle_CacheLookup_ReturnsNullIfNotCacheEnabled()
        {
            var mockToggleCacheProvider = new Mock<ICacheToggleProvider>();
            mockToggleCacheProvider.Setup(p => p.IsCacheEnabled).Returns(false);
            var dynamicStyleBundle = new DynamicStyleBundle("~/common/styles", new DefaultCacheKeyGenerator(), mockToggleCacheProvider.Object);
            var context = new BundleContext(new Mock<HttpContextBase>().Object, new BundleCollection(), "/common/styles");

            var response = dynamicStyleBundle.CacheLookup(context);

            Assert.That(response, Is.Null);
            mockToggleCacheProvider.VerifyGet(p => p.IsCacheEnabled, Times.Once);
        }


        [Test]
        public void DynamicStyleBundle_CacheLookup_ReturnsBaseCacheIfCachingisEnabled()
        {
            var mockToggleCacheProvider = new Mock<ICacheToggleProvider>();
            mockToggleCacheProvider.Setup(p => p.IsCacheEnabled).Returns(true);
            var dynamicStyleBundle = new DynamicStyleBundle("~/common/styles", new DefaultCacheKeyGenerator(), mockToggleCacheProvider.Object);
            var context = new BundleContext(new Mock<HttpContextBase>().Object, new BundleCollection(), "/common/styles");

            var response = dynamicStyleBundle.CacheLookup(context);

            Assert.That(response, Is.Null);
            mockToggleCacheProvider.VerifyGet(p => p.IsCacheEnabled, Times.Once);
        }

        [Test]
        public void DynamicStyleBundle_GenerateBundleResponse()
        {
            var bundle = new DynamicStyleBundle("~/common/styles");
            var context = new BundleContext(
                new Mock<HttpContextBase>().Object,
                new BundleCollection() { bundle },
                "/common/styles");

            var response = bundle.GenerateBundleResponse(context);

            Assert.That(response.Cacheability, Is.EqualTo(HttpCacheability.Public));
            Assert.That(response.ContentType, Is.Null);
            Assert.That(response.Content, Is.Empty);
            Assert.That(response.Files, Is.Not.Null);
            Assert.That(response.Files.Count(), Is.EqualTo(0));
        }
    }
}
