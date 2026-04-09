using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.EnumExtensions;

namespace Extensions.Tests.EnumExtensions
{
    [TestClass]
    public class EnumItemTests
    {
        [TestMethod]
        public void GetAllItems()
        {
            var items = EnumItems.GetAllItems<Tester>().ToArray();
            CollectionAssert.AreEqual(new[] {Tester.A, Tester.B, Tester.LongName}, items);
        }

        private enum Tester
        {
            A,
            B,
            LongName
        }
    }
}