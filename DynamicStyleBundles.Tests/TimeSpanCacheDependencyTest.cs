namespace DynamicStyleBundles.Tests
{
    using System;
    using System.Threading;
    using NUnit.Framework;

    [TestFixture]
    public class TimeSpanCacheDependencyTest
    {
        [Test]
        public void TimeSpanCacheDependency_ZeroOrNegativeAbsoluteExpiration_Throws([Values(-1, 0)]int value)
        {
            Assert.That(() => new TimeSpanCacheDependency(value), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void TimeSpanCacheDependency_Expires()
        {
            using (var dep = new TimeSpanCacheDependency(5))
            {
                Assert.That(dep.HasChanged, Is.False);
                Thread.Sleep(6000);
                Assert.That(dep.HasChanged, Is.True);
            }
        }
    }
}
