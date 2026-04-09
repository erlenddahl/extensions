using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Extensions.Tests.IEnumerableExtensions
{
    [TestClass]
    public class LeastCommonMultipleExtensions
    {
        [TestMethod]
        public void ComputeLCM_SingleNumber_ShouldReturnSameNumber()
        {
            var numbers = new List<int> { 7 };
            var result = numbers.ComputeLeastCommonMultiple(n => n);
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void ComputeLCM_TwoNumbers_ShouldReturnLCM()
        {
            var numbers = new List<int> { 12, 18 };
            var result = numbers.ComputeLeastCommonMultiple(n => n);
            Assert.AreEqual(36, result);
        }

        [TestMethod]
        public void ComputeLCM_MultipleNumbers_ShouldReturnLCM()
        {
            var numbers = new List<int> { 2, 5, 10, 15, 20 };
            var result = numbers.ComputeLeastCommonMultiple(n => n);
            Assert.AreEqual(60, result);
        }

        [TestMethod]
        public void ComputeLCM_EqualNumbers_ShouldReturnSameNumber()
        {
            var numbers = new List<int> { 5, 5, 5, 5, 5 };
            var result = numbers.ComputeLeastCommonMultiple(n => n);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void ComputeLCM_NonEqualNumbers_ShouldReturnLCM()
        {
            var numbers = new List<int> { 3, 7, 14, 21 };
            var result = numbers.ComputeLeastCommonMultiple(n => n);
            Assert.AreEqual(42, result);
        }
    }
}