namespace DynamicStyleBundles.Tests
{
    using System;
    using System.Text;
    using System.IO;

    using NUnit.Framework;

    [TestFixture]
    public class CustomVirtualFileTest
    {
        [Test]
        public void ContentVirutalFile_Constructor_NullContent_Throws()
        {
            Assert.That(() => new CustomVirtualFile("a.txt", null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ContentVirutalFile_GetStream_Returns_ContentData()
        {
            var data = Encoding.UTF8.GetBytes("ABCDEF");
            var virtualFile = new CustomVirtualFile("a.txt", new Asset(data, DateTime.Now));
            Assert.That(virtualFile.Name, Is.EqualTo("a.txt"));
            Assert.That(virtualFile.VirtualPath, Is.EqualTo("a.txt"));
            Assert.That(virtualFile.IsDirectory, Is.False);

            var stream = virtualFile.Open();
            Assert.That(stream, Is.Not.Null);
            Assert.That(stream, Is.EqualTo(new MemoryStream(data)));
        }
    }
}
