using Extensions.DoubleExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.DoubleExtensions;

[TestClass]
public class PercentageDifferenceTests
{
    [TestMethod]
    public void SimpleDifferenceTest()
    {
        Assert.AreEqual(1.88, 1.05.PercentageDifference(1.07), 0.01);
        Assert.AreEqual(1.88, 1.07.PercentageDifference(1.05), 0.01);
    }

    [TestMethod]
    public void LargeDifference()
    {
        Assert.AreEqual(200, 53662d.PercentageDifference(1.07), 0.01);
    }

    [TestMethod]
    public void IsSimilar()
    {
        Assert.AreEqual(true, 1.05.IsSimilar(1.07));
    }

    [TestMethod]
    public void IsNotSimilar()
    {
        Assert.AreEqual(false, 1.05.IsSimilar(1.37));
    }
}