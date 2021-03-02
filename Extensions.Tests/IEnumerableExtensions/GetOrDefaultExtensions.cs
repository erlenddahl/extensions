using Extensions.IEnumerableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class GetOrDefaultExtensions
    {
        [TestMethod]
        public void GetOrDefaultTests()
        {
            var arr = new[] {51, 1, 41, 13, 31.1};
            for (var i = 0; i < arr.Length; i++)
                Assert.AreEqual(arr[i], arr.GetOrDefault(i));

            Assert.AreEqual(default(int), arr.GetOrDefault(-1));
            Assert.AreEqual(default(int), arr.GetOrDefault(5));
            Assert.AreEqual(default(int), arr.GetOrDefault(100));

            Assert.AreEqual(int.MinValue, arr.GetOrDefault(-1, int.MinValue));
            Assert.AreEqual(int.MinValue, arr.GetOrDefault(5, int.MinValue));
            Assert.AreEqual(int.MinValue, arr.GetOrDefault(100, int.MinValue));
        }
    }
}