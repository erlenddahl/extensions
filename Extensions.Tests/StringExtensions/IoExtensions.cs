using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.StringExtensions;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class IoExtensions
    {

        [TestMethod]
        public void MakeSafeForFilenameTests()
        {
            Assert.AreEqual("hei", "hei".MakeSafeForFilename());
            Assert.AreEqual("", "".MakeSafeForFilename());
            Assert.AreEqual("h--ei--", "h?:ei?:".MakeSafeForFilename("-"));
            Assert.AreEqual("hei", "h?:ei?:".MakeSafeForFilename());
            Assert.AreEqual("hei", "h**///\\\\?:e<>i?:".MakeSafeForFilename());
        }

        [TestMethod]
        public void ChangeExtensionTests()
        {
            Assert.AreEqual("", "".ChangeExtension(".ldb"));
            Assert.AreEqual(null, ((string)null).ChangeExtension(".ldb"));

            Assert.AreEqual("C:\\test.ldb", "C:\\test".ChangeExtension(".ldb"));

            Assert.AreEqual("C:\\test.ldb", "C:\\test.mdb".ChangeExtension(".ldb"));
            Assert.AreEqual("C:\\te.s.t.ldb", "C:\\te.s.t.mdb".ChangeExtension(".ldb"));
            Assert.AreEqual("C:\\te\\s\\t.ldb", "C:\\te\\s\\t.mdb".ChangeExtension(".ldb"));
        }

        [TestMethod]
        public void ChangeExtensionFromNoExtension()
        {
            Assert.AreEqual("C:\\a\\b\\c.ldb", "C:\\a\\b\\c".ChangeExtension(".ldb"));
            Assert.AreEqual("C:\\a.txt\\b\\c.ldb", "C:\\a.txt\\b\\c".ChangeExtension(".ldb"));
            Assert.AreEqual("C:\\a\\b.txt\\c.ldb", "C:\\a\\b.txt\\c".ChangeExtension(".ldb"));
        }

        [TestMethod]
        public void RemoveExtensionTests()
        {
            Assert.AreEqual("", "".RemoveExtension());
            Assert.AreEqual(null, ((string)null).RemoveExtension());

            Assert.AreEqual("C:\\test", "C:\\test".RemoveExtension());

            Assert.AreEqual("C:\\test", "C:\\test.mdb".RemoveExtension());
            Assert.AreEqual("C:\\te.s.t", "C:\\te.s.t.mdb".RemoveExtension());
            Assert.AreEqual("C:\\te.faef\\fact", "C:\\te.faef\\fact.mdb".RemoveExtension());
            Assert.AreEqual("C:\\te\\s\\t", "C:\\te\\s\\t.mdb".RemoveExtension());
        }

        [TestMethod]
        public void TruncatePathTests()
        {
            Assert.AreEqual(@"C:\folder\one\two\three\file.txt", @"C:\folder\one\two\three\file.txt".TruncatePath(500));
            Assert.AreEqual(@"C:\folder\one\two\three\file.txt", @"C:\folder\one\two\three\file.txt".TruncatePath(35));
            Assert.AreEqual(@"C:\folder\one\...\file.txt", @"C:\folder\one\two\three\file.txt".TruncatePath(27));
            Assert.AreEqual(@"C:\folder\...\file.txt", @"C:\folder\one\two\three\file.txt".TruncatePath(23));
            Assert.AreEqual(@"C:\...\file.txt", @"C:\folder\one\two\three\file.txt".TruncatePath(15));
            Assert.AreEqual(@"C:\...\file.txt", @"C:\folder\one\two\three\file.txt".TruncatePath(10));
            Assert.AreEqual(@"C:\...\file.txt", @"C:\folder\one\two\three\file.txt".TruncatePath(5));
        }

        [TestMethod]
        public void AddSuffixBeforeExtension()
        {
            Assert.AreEqual(@"C:\folder\one\two\three\file-123.txt", @"C:\folder\one\two\three\file.txt".AddSuffixBeforeExtension("-123"));
            Assert.AreEqual(@"C:\folder\one\two\three\file.txt", @"C:\folder\one\two\three\file.txt".AddSuffixBeforeExtension(""));
            Assert.AreEqual(@"C:\folder\one\two\three\file.tar-123.gz", @"C:\folder\one\two\three\file.tar.gz".AddSuffixBeforeExtension("-123"));
        }
    }
}