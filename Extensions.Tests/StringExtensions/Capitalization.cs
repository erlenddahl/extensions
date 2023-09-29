using Extensions.StringExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class Capitalization
    {
        [TestMethod]
        public void CapitalizeFirstTests()
        {
            Assert.AreEqual("", "".CapitalizeFirst());
            Assert.AreEqual("John", "John".CapitalizeFirst());
            Assert.AreEqual("John", "john".CapitalizeFirst());
            Assert.AreEqual("A", "a".CapitalizeFirst());
            Assert.AreEqual("A", "A".CapitalizeFirst());
            Assert.AreEqual("ABC", "ABC".CapitalizeFirst());
            Assert.AreEqual("ABC", "aBC".CapitalizeFirst());
        }

        [TestMethod]
        public void ToLowerFirstTests()
        {
            Assert.AreEqual("", "".ToLowerFirst());
            Assert.AreEqual("john", "John".ToLowerFirst());
            Assert.AreEqual("john", "john".ToLowerFirst());
            Assert.AreEqual("a", "a".ToLowerFirst());
            Assert.AreEqual("a", "A".ToLowerFirst());
            Assert.AreEqual("aBC", "ABC".ToLowerFirst());
            Assert.AreEqual("aBC", "aBC".ToLowerFirst());
        }

        [TestMethod]
        public void ToCamelCaseTests()
        {
            Assert.AreEqual("hei", "Hei".ToCamelCase());
            Assert.AreEqual("heiOgHallo", "Hei og hallo".ToCamelCase());
            Assert.AreEqual("hei", "262Hei,.,,-.,".ToCamelCase());
            Assert.AreEqual("hybridElectricGasoline", "Hybrid, electric/gasoline".ToCamelCase());
        }
    }
}