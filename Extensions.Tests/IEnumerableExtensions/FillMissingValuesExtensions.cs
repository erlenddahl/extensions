using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.IEnumerableExtensions;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class FillMissingValuesExtensions
    {
        [TestMethod]
        public void SimpleHoles()
        {
            var values = new[] { 1, 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0 };
            var replaced = values.FillMissingValues(p => p != 0, p => p, (p, i, v) => values[i] = v);
            Assert.AreEqual(5, replaced);
            CollectionAssert.AreEqual(new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 }, values);
        }

        [TestMethod]
        public void DifferentValues()
        {
            var values = new[] { 1, 0, 0, 0, 0, 2, 1, 0, 1, 0, 2, 0, 2, 0 };
            var replaced = values.FillMissingValues(p => p != 0, p => p, (p, i, v) => values[i] = v);
            Assert.AreEqual(2, replaced);
            CollectionAssert.AreEqual(new[] { 1, 0, 0, 0, 0, 2, 1, 1, 1, 0, 2, 2, 2, 0 }, values);
        }
    }
}
