namespace DynamicStyleBundles.Tests
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Web;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class HttpHandlerTest
    {

        [Test]
        public void HttpHandler_Constructor_WithNullAssetLoarder_Throws_ArgumentNullExcpetion()
        {
            Assert.That(() => new HttpHandler(null, "ABC"), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void HttpHandler_Constructor_WithNullOrEmptyFallbackDirectoryName_Throws_ArgumentNullExcpetion([Values(null, "")] string value)
        {
            Assert.That(() => new HttpHandler(() => new Mock<IAssetLoader>().Object, value), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void HttpHandler_IsReusable_Returns_True()
        {
            var handler = new HttpHandler(() => new Mock<IAssetLoader>().Object, "ABC");
            Assert.That(handler.IsReusable, Is.True);
        }

        [Test]
        public void HttpHandler_NotRegisteredContentRetriever_Throws_NotSupportedException()
        {
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.SetupGet(r => r.Path).Returns("/Content/Images/picto/check.gif");
            mockRequest.SetupGet(r => r.QueryString).Returns(new NameValueCollection());

            var mockCtx = new Mock<HttpContextBase>();
            mockCtx.SetupGet(c => c.Request).Returns(mockRequest.Object);

            var mockResponse = new Mock<HttpResponseBase>();
            mockCtx.SetupGet(c => c.Response).Returns(mockResponse.Object);

            var handler = new HttpHandler(() => null, "Content");
            Assert.That(
                () => handler.ProcessRequestInternal(mockCtx.Object),
                Throws.InstanceOf<NotSupportedException>().With.Message.EqualTo("Unable to get a not null instance of IContentRetriever."));
        }

        [Test]
        public void HttpHandler_NullContent_Returns_404()
        {
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.SetupGet(r => r.Path).Returns("/Content/Images/picto/check.gif");
            mockRequest.SetupGet(r => r.ApplicationPath).Returns("/");
            mockRequest.SetupGet(r => r.QueryString).Returns(new NameValueCollection());

            var mockCtx = new Mock<HttpContextBase>();
            mockCtx.SetupGet(c => c.Request).Returns(mockRequest.Object);
            var mockResponse = new Mock<HttpResponseBase>();
            mockCtx.SetupGet(c => c.Response).Returns(mockResponse.Object);

            var handler = new HttpHandler(() => new Mock<IAssetLoader>().Object, "Content");
            handler.ProcessRequestInternal(mockCtx.Object);

            mockResponse.VerifySet(r => r.StatusCode = 404, Times.Once);
        }

        [Test]
        public void HttpHandler_NotNullContent_ReturnsFile()
        {
            AssertInconclusiveIfInDebugMode();

            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.SetupGet(r => r.Path).Returns("/Content/Images/picto/check.gif");
            mockRequest.SetupGet(r => r.AppRelativeCurrentExecutionFilePath).Returns("~/");
            mockRequest.SetupGet(r => r.ApplicationPath).Returns("/");
            mockRequest.SetupGet(r => r.QueryString).Returns(new NameValueCollection());

            var mockCtx = new Mock<HttpContextBase>();
            mockCtx.SetupGet(c => c.Request).Returns(mockRequest.Object);

            var mockResponse = new Mock<HttpResponseBase>();
            mockCtx.SetupGet(c => c.Response).Returns(mockResponse.Object);

            var mockCachePolicy = new Mock<HttpCachePolicyBase>();
            mockResponse.SetupGet(r => r.Cache).Returns(mockCachePolicy.Object);

            var headers = new HttpCacheVaryByHeaders();
            mockCachePolicy.SetupGet(c => c.VaryByHeaders).Returns(headers);

            var varyByParams = new HttpCacheVaryByParams();
            mockCachePolicy.SetupGet(c => c.VaryByParams).Returns(varyByParams);

            var contentRetriever = new FakeAssetLoader();
            var handler = new HttpHandler(() => contentRetriever, "Content");
            handler.ProcessRequestInternal(mockCtx.Object);

            mockResponse.VerifySet(r => r.ContentType = "image/gif", Times.Once);
            mockResponse.Verify(r => r.BinaryWrite(It.Is<byte[]>(b => b == contentRetriever.RawData)), Times.Once());
            mockCachePolicy.Verify(c => c.SetCacheability(HttpCacheability.Public), Times.Once);
            mockCachePolicy.Verify(c => c.SetExpires(It.Is<DateTime>(d => DateTime.Now.AddHours(1.0) - d < TimeSpan.FromSeconds(5))), Times.Once);
            mockCachePolicy.Verify(c => c.SetLastModified(It.Is<DateTime>(d => d == contentRetriever.LastModified)), Times.Once);
            Assert.That(headers["Host"], Is.EqualTo(true));
        }

        [Test]
        public void HttpHandler_GetContentByQueryString_ReturnsFile()
        {
            AssertInconclusiveIfInDebugMode();

            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.SetupGet(r => r.Path).Returns("/Content/");
            mockRequest.SetupGet(r => r.ApplicationPath).Returns("/");
            var nameValueCollection = new NameValueCollection();
            nameValueCollection.Add("resource", "/Images/picto/check.gif");
            mockRequest.SetupGet(r => r.QueryString).Returns(nameValueCollection);

            var mockCtx = new Mock<HttpContextBase>();
            mockCtx.SetupGet(c => c.Request).Returns(mockRequest.Object);

            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.SetupProperty(p => p.ContentType);
            mockCtx.SetupGet(c => c.Response).Returns(mockResponse.Object);

            var mockCachePolicy = new Mock<HttpCachePolicyBase>();
            mockResponse.SetupGet(r => r.Cache).Returns(mockCachePolicy.Object);

            var headers = new HttpCacheVaryByHeaders();
            mockCachePolicy.SetupGet(c => c.VaryByHeaders).Returns(headers);

            var varyByParams = new HttpCacheVaryByParams();
            mockCachePolicy.SetupGet(c => c.VaryByParams).Returns(varyByParams);

            var contentRetriever = new FakeAssetLoader();
            var handler = new HttpHandler(() => contentRetriever, "Content");
            handler.ProcessRequestInternal(mockCtx.Object);

            mockResponse.VerifySet(r => r.ContentType = "image/gif", Times.Once);
            mockResponse.Verify(r => r.BinaryWrite(It.Is<byte[]>(b => b == contentRetriever.RawData)), Times.Once());
            mockCachePolicy.Verify(c => c.SetCacheability(HttpCacheability.Public), Times.Once);
            mockCachePolicy.Verify(c => c.SetExpires(It.Is<DateTime>(d => DateTime.Now.AddHours(1.0) - d < TimeSpan.FromSeconds(5))), Times.Once);
            mockCachePolicy.Verify(c => c.SetLastModified(It.Is<DateTime>(d => d == contentRetriever.LastModified)), Times.Once);
            Assert.That(headers["Host"], Is.EqualTo(true));
        }

        [Conditional("DEBUG")]
        private static void AssertInconclusiveIfInDebugMode()
        {
            Assert.Inconclusive("This test can only run in RELEASE mode");
        }

        class FakeAssetLoader : IAssetLoader
        {
            public readonly byte[] RawData;
            public readonly DateTime LastModified;

            public FakeAssetLoader()
            {
                this.LastModified = DateTime.Now.AddDays(-10);
                this.RawData = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            }

            public Asset Load(string filePath)
            {
                if (filePath == "/Images/picto/check.gif")
                {
                    return new Asset(RawData, LastModified);
                }
                return null;
            }
        }
    }
}
