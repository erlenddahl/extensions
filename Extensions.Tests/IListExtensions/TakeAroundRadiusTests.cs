using Extensions.IListExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.IListExtensions;

[TestClass]
public class TakeAroundRadiusTests
{
    [TestMethod]
    public void Simple()
    {
        var list = new[] { 5, 5, 5, 5, 4, 2, 4, 6, 7, 1, 3, 3, 8, 1, 3, 5, 7, 2, 4 };
        CollectionAssert.AreEqual(new[] { 5, 5, 4, 2, 4 }, list.TakeAround(4, 2));
    }

    [TestMethod]
    public void CollisionStart()
    {
        var list = new[] { 5, 5, 5, 5, 4, 2, 4, 6, 7, 1, 3, 3, 8, 1, 3, 5, 7, 2, 4 };
        CollectionAssert.AreEqual(new[] { 5, 5, 5, 5, 4, 2, 4 }, list.TakeAround(2, 4));
    }


    [TestMethod]
    public void CollisionEnd()
    {
        var list = new[] { 5, 5, 5, 5, 4, 2, 4, 6, 7, 1, 3, 3, 8, 1, 3, 5, 7, 2, 4 };
        CollectionAssert.AreEqual(new[] { 1, 3, 5, 7, 2, 4 }, list.TakeAround(16, 3));
    }


    [TestMethod]
    public void CollisionBoth()
    {
        var list = new[] { 5, 5, 5, 5, 4, 2, 4, 6, 7, 1, 3, 3, 8, 1, 3, 5, 7, 2, 4 };
        CollectionAssert.AreEqual(list, list.TakeAround(10, 300));
    }
}