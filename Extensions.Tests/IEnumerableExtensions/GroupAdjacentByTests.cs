using System.Linq;
using Extensions.IEnumerableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IEnumerableExtensions;

[TestClass]
public class GroupAdjacentByTests
{
    [TestMethod]
    public void SingleGroup()
    {
        var items = new[] { 1, 1, 1, 1, 1, 1 };
        var groups = items.GroupAdjacentBy((a, b) => a == b).ToArray();

        Assert.AreEqual(1, groups.Length);
        Assert.AreEqual(6, groups[0].Count);
    }

    [TestMethod]
    public void TwoGroups()
    {
        var items = new[] { 1, 1, 1, 1, 2, 2 };
        var groups = items.GroupAdjacentBy((a, b) => a == b).ToArray();

        Assert.AreEqual(2, groups.Length);
        Assert.AreEqual(4, groups[0].Count);
        Assert.AreEqual(2, groups[1].Count);
    }

    [TestMethod]
    public void MoreGroups()
    {
        var items = new[] { 1, 2, 3, 4, 5, 6 };
        var groups = items.GroupAdjacentBy((a, b) => a == b).ToArray();

        Assert.AreEqual(6, groups.Length);
        Assert.AreEqual(1, groups[0].Count);
        Assert.AreEqual(1, groups[1].Count);
        Assert.AreEqual(1, groups[2].Count);
        Assert.AreEqual(1, groups[3].Count);
        Assert.AreEqual(1, groups[4].Count);
        Assert.AreEqual(1, groups[5].Count);
    }
}

[TestClass]
public class GroupAdjacentBy_IncludeFirstMistMatch_Tests
{
    [TestMethod]
    public void SingleGroup()
    {
        var items = new[] { 1, 1, 1, 1, 1, 1 };
        var groups = items.GroupAdjacentBy((a, b) => a == b, true).ToArray();

        Assert.AreEqual(1, groups.Length);
        Assert.AreEqual(6, groups[0].Count);
    }

    [TestMethod]
    public void TwoGroups()
    {
        var items = new[] { 1, 1, 1, 1, 2, 2 };
        var groups = items.GroupAdjacentBy((a, b) => a == b, true).ToArray();

        Assert.AreEqual(2, groups.Length);
        Assert.AreEqual(5, groups[0].Count);
        Assert.AreEqual(2, groups[1].Count);
    }

    [TestMethod]
    public void MoreGroups()
    {
        var items = new[] { 1, 2, 3, 4, 5, 6 };
        var groups = items.GroupAdjacentBy((a, b) => a == b, true).ToArray();

        Assert.AreEqual(6, groups.Length);
        Assert.AreEqual(2, groups[0].Count);
        Assert.AreEqual(2, groups[1].Count);
        Assert.AreEqual(2, groups[2].Count);
        Assert.AreEqual(2, groups[3].Count);
        Assert.AreEqual(2, groups[4].Count);
        Assert.AreEqual(1, groups[5].Count);
    }
}