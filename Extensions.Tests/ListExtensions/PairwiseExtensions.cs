using System.Linq;
using Extensions.ListExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.ListExtensions
{
    [TestClass]
    public class PairwiseExtensions
    {
        [TestMethod]
        public void Pairwise()
        {
            var data = "a b c d e f g h i j".Split(' ');
            var pairs = data.Pairwise().ToList();

            Assert.AreEqual(9, pairs.Count);

            Assert.AreEqual("a", pairs[0].Item1);
            Assert.AreEqual("b", pairs[1].Item1);
            Assert.AreEqual("c", pairs[2].Item1);
            Assert.AreEqual("d", pairs[3].Item1);
            Assert.AreEqual("e", pairs[4].Item1);
            Assert.AreEqual("f", pairs[5].Item1);
            Assert.AreEqual("g", pairs[6].Item1);
            Assert.AreEqual("h", pairs[7].Item1);
            Assert.AreEqual("i", pairs[8].Item1);

            Assert.AreEqual("b", pairs[0].Item2);
            Assert.AreEqual("c", pairs[1].Item2);
            Assert.AreEqual("d", pairs[2].Item2);
            Assert.AreEqual("e", pairs[3].Item2);
            Assert.AreEqual("f", pairs[4].Item2);
            Assert.AreEqual("g", pairs[5].Item2);
            Assert.AreEqual("h", pairs[6].Item2);
            Assert.AreEqual("i", pairs[7].Item2);
            Assert.AreEqual("j", pairs[8].Item2);
        }

        [TestMethod]
        public void PairwiseAggregated()
        {
            var data = "a b c d e f g h i j".Split(' ');
            var pairs = data.Pairwise((a, b) => a + b).ToList();

            Assert.AreEqual(9, pairs.Count);

            Assert.AreEqual("ab", pairs[0]);
            Assert.AreEqual("bc", pairs[1]);
            Assert.AreEqual("cd", pairs[2]);
            Assert.AreEqual("de", pairs[3]);
            Assert.AreEqual("ef", pairs[4]);
            Assert.AreEqual("fg", pairs[5]);
            Assert.AreEqual("gh", pairs[6]);
            Assert.AreEqual("hi", pairs[7]);
            Assert.AreEqual("ij", pairs[8]);
        }
    }
}
