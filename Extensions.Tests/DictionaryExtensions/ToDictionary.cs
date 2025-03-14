using Extensions.DictionaryExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.DictionaryExtensions
{
    [TestClass]
    public class ToDictionary
    {
        [TestMethod]
        public void ToDictionarySafe_WithoutDuplicates()
        {
            var arr = new int[] {1, 2, 3, 4, 5};
            var d = arr.ToDictionarySafe(k => k.ToString(), v => v);

            Assert.AreEqual(5, d.Count);
            for (var i = 1; i < 6; i++)
                Assert.AreEqual(i, d[i.ToString()]);
        }

        [TestMethod]
        public void ToDictionarySafe_Duplicates_AppendNumbers()
        {
            var arr = new int[] { 1, 2, 3, 4, 5, 5 };
            var d = arr.ToDictionarySafe(k => k.ToString(), v => v, DictionaryDuplicateKeyHandling.AppendNumbers);

            Assert.AreEqual(6, d.Count);
            for (var i = 1; i < 6; i++)
                Assert.AreEqual(i, d[i.ToString()]);

            Assert.AreEqual(5, d["5 (2)"]);
        }

        [TestMethod]
        public void ToDictionarySafe_Duplicates_UseFirst()
        {
            var arr = new int[] { 1, 2, 3, 4, 5, 5 };
            var d = arr.ToDictionarySafe(k => k.ToString(), v => v, DictionaryDuplicateKeyHandling.UseFirstValue);

            Assert.AreEqual(5, d.Count);
            for (var i = 1; i < 6; i++)
                Assert.AreEqual(i, d[i.ToString()]);

        }
    }
}