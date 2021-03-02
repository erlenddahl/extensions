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
            Assert.AreEqual("",       "".CapitalizeFirst());
            Assert.AreEqual("John",   "John".CapitalizeFirst());
            Assert.AreEqual("John",   "john".CapitalizeFirst());
            Assert.AreEqual("A",      "a".CapitalizeFirst());
            Assert.AreEqual("ABC",    "ABC".CapitalizeFirst());
            Assert.AreEqual("ABC",    "aBC".CapitalizeFirst());
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