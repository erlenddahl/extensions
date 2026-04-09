using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.IListExtensions;

namespace Extensions.Tests.IListExtensions
{
    [TestClass]
    public class TakeAroundExtensions
    {

        [TestMethod]
        public void TakeAroundTests()
        {
            var list = new[] {5, 5, 5, 5, 4, 2, 4, 6, 7, 1, 3, 3, 8, 1, 3, 5, 7, 2, 4};

            var res = list.TakeAround(0, p => false, p => false).ToList();
            Assert.AreEqual(1, res.Count);
            Assert.AreEqual(5, res[0]);

            //Take 5's
            for (var i = 0; i < 4; i++)
            {
                res = list.TakeAround(i, p => p == 5, p => p == 5).ToList();
                Assert.AreEqual(4, res.Count);
                for (var j = 0; j < res.Count; j++)
                    Assert.AreEqual(5, res[j]);
            }

            res = list.TakeAround(4, p => p == 5, p => p == 5).ToList();
            Assert.AreEqual(5, res.Count);
            for (var j = 0; j < res.Count - 1; j++)
                Assert.AreEqual(5, res[j]);
            Assert.AreEqual(4, res.Last());

            res = list.TakeAround(5, p => p == 5, p => p == 5).ToList();
            Assert.AreEqual(1, res.Count);
            Assert.AreEqual(2, res[0]);

            //Take none
            for (var i = 0; i < list.Length; i++)
            {
                res = list.TakeAround(i, p => false, p => false).ToList();
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual(list[i], res[0]);
            }

            //Take all
            for (var i = 0; i < list.Length; i++)
            {
                res = list.TakeAround(i, p => true, p => true).ToList();
                Assert.AreEqual(list.Length, res.Count);
                for (var j = 0; j < list.Length; j++)
                    Assert.AreEqual(list[j], res[j]);
            }

            //Take before
            for (var i = 0; i < list.Length; i++)
            {
                res = list.TakeAround(i, p => true, p => false).ToList();
                Assert.AreEqual(i + 1, res.Count);
                for (var j = 0; j < res.Count; j++)
                    Assert.AreEqual(list[j], res[j]);
            }

            //Take after
            for (var i = 0; i < list.Length; i++)
            {
                res = list.TakeAround(i, p => false, p => true).ToList();
                Assert.AreEqual(list.Length - i, res.Count);
                for (var j = i; j < list.Length; j++)
                    Assert.AreEqual(list[j], res[j-i]);
            }
        }
    }
}