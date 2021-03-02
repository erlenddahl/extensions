using System.Linq;
using Extensions.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Utilities
{
    [TestClass]
    public class EnumHelperTests
    {
        public enum TestEnum
        {
            A, B, C
        }

        [TestMethod]
        public void GetAllItems()
        {
            var items = EnumHelper.GetValues<TestEnum>().ToArray();

            Assert.AreEqual(3, items.Length);
            Assert.AreEqual(TestEnum.A, items[0]);
            Assert.AreEqual(TestEnum.B, items[1]);
            Assert.AreEqual(TestEnum.C, items[2]);
        }

        [TestMethod]
        public void Parse_CorrectCasing()
        {
            Assert.AreEqual(TestEnum.A, EnumHelper.Parse<TestEnum>("A"));
            Assert.AreEqual(TestEnum.B, EnumHelper.Parse<TestEnum>("B"));
            Assert.AreEqual(TestEnum.C, EnumHelper.Parse<TestEnum>("C"));
        }

        [TestMethod]
        public void Parse_LowerCase()
        {
            Assert.AreEqual(TestEnum.A, EnumHelper.Parse<TestEnum>("a"));
            Assert.AreEqual(TestEnum.B, EnumHelper.Parse<TestEnum>("b"));
            Assert.AreEqual(TestEnum.C, EnumHelper.Parse<TestEnum>("c"));
        }

        [TestMethod]
        public void Parse_Number()
        {
            Assert.AreEqual(TestEnum.A, EnumHelper.Parse<TestEnum>("0"));
            Assert.AreEqual(TestEnum.B, EnumHelper.Parse<TestEnum>("1"));
            Assert.AreEqual(TestEnum.C, EnumHelper.Parse<TestEnum>("2"));
        }
    }
}