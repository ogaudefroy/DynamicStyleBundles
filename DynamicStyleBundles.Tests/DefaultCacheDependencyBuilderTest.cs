namespace DynamicStyleBundles.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class DefaultCacheDependencyBuilderTest
    {
        [Test]
        public void DefaultCacheDependencyBuilder_GetCacheDependency_Returns_TimeSpanCacheDependency_Test()
        {
            var builder = new DefaultCacheDependencyBuilder();

            var cacheDependency = builder.GetCacheDependency("~/dynamiccontent/asset", null, DateTime.UtcNow);

            Assert.That(cacheDependency, Is.Not.Null);
            Assert.That(cacheDependency, Is.InstanceOf<TimeSpanCacheDependency>());
        }
    }
}
