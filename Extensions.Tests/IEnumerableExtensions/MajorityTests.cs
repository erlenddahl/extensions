using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.IEnumerableExtensions;

namespace Extensions.Tests.IEnumerableExtensions;

[TestClass]
public class MajorityTests
{
    [TestMethod]
    public void SimpleMajorityTests()
    {
        Assert.AreEqual(2, new[] { 1, 2, 2, 3, 4, 5 }.Majority());
        Assert.AreEqual(2, new[] { 1, 2, 2, 3, 3, 4, 5 }.Majority());
        Assert.AreEqual(2, new[] { 1, 2, 2, 2, 3, 4, 4, 5, 5 }.Majority());
        Assert.AreEqual(2, new[] { 1, 2, 2, 3, 4, 5, 2, 5, 4, 5, 4, 2, 2, 1, 2 }.Majority());
    }

    [TestMethod]
    public void MajorityAroundTests()
    {
        var arr = new[] { 1, 2, 2, 3, 4, 5, 2, 5, 4, 5, 4, 2, 2, 1, 2 };
        Assert.AreEqual(2, arr.MajorityAround(0, 3));
        Assert.AreEqual(2, arr.MajorityAround(1, 3));
        Assert.AreEqual(2, arr.MajorityAround(2, 3));
        Assert.AreEqual(2, arr.MajorityAround(3, 3));
        Assert.AreEqual(5, arr.MajorityAround(6, 3));
    }

    [TestMethod]
    public void MajoritySmoothTest()
    {
        var arr = new[] { 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1 };
        var cor = new[] { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        var smoothed = arr.MajoritySmooth(3);

        CollectionAssert.AreEqual(cor, smoothed);
    }
}