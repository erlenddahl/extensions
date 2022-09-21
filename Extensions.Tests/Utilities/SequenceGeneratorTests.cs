using System.Linq;
using Extensions.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Utilities
{
    [TestClass]
    public class SequenceGeneratorTests
    {
        [TestMethod]
        public void Simple()
        {
            var arr = SequenceGenerator.Start(0, 5);
            var correct = new[] { 0, 1, 2, 3, 4 };

            CollectionAssert.AreEqual(correct, arr.ToArray());
        }

        [TestMethod]
        public void Step()
        {
            var arr = SequenceGenerator.Start(0, 5, 2);
            var correct = new[] { 0, 2, 4, 6, 8 };

            CollectionAssert.AreEqual(correct, arr.ToArray());
        }

        [TestMethod]
        public void MultipleSteps()
        {
            var arr = SequenceGenerator.Start(0, 5).Then(5);
            var correct = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            CollectionAssert.AreEqual(correct, arr.ToArray());
        }

        [TestMethod]
        public void MultipleSteps_DifferentStep()
        {
            var arr = SequenceGenerator.Start(0, 5).Then(5, 2);
            var correct = new[] { 0, 1, 2, 3, 4, 6, 8, 10, 12, 14 };

            CollectionAssert.AreEqual(correct, arr.ToArray());
        }

        [TestMethod]
        public void Complex()
        {
            var arr = SequenceGenerator.Start(0, 11).Then(4, 5).Then(2, 10).Then(2, 25);
            var correct = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 20, 25, 30, 40, 50, 75, 100};

            CollectionAssert.AreEqual(correct, arr.ToArray());
        }
    }
}