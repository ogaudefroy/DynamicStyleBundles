namespace DynamicStyleBundles.Tests
{
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DefaultCacheKeyGeneratorTest
    {
        [Test]
        public void DefaultCacheKeyGenerator_GetCacheKey_Returns_CompositeKeyWithHttpHost()
        {
            var nvc = new NameValueCollection() { { "HTTP_HOST", "MYSITE" } };
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.SetupGet(p => p.ServerVariables).Returns(nvc);
            var mockContext = new Mock<HttpContextBase>();
            mockContext.SetupGet(p => p.Request).Returns(mockRequest.Object);
            var provider = new DefaultCacheKeyGenerator();

            var generatedKey = provider.GetCacheKey(mockContext.Object, "TEST");

            Assert.That(generatedKey, Is.EqualTo("MYSITE:TEST"));
        }

        [Test]
        public void DefaultCacheKeyGenerator_GetCacheKey_Returns_CompositeKeyWithDnsSafeHost()
        {
            var url = new Uri("http://myurl");
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.SetupGet(p => p.ServerVariables).Returns(new NameValueCollection());
            mockRequest.SetupGet(p => p.Url).Returns(url);
            var mockContext = new Mock<HttpContextBase>();
            mockContext.SetupGet(p => p.Request).Returns(mockRequest.Object);
            var provider = new DefaultCacheKeyGenerator();

            var generatedKey = provider.GetCacheKey(mockContext.Object, "TEST");

            Assert.That(generatedKey, Is.EqualTo("myurl:TEST"));
        }
    }
}
