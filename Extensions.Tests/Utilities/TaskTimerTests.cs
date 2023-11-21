using System.Threading.Tasks;
using Extensions.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Utilities;

[TestClass]
public class TaskTimerTests
{
    [TestMethod]
    public async Task ContentsTest()
    {
        var timer = new TaskTimer();
        timer.Time("one");
        timer.Time("two");

        Assert.AreEqual(2, timer.Timings.Count);

        Assert.IsTrue(timer.Timings.ContainsKey("one"));
        Assert.IsTrue(timer.Timings.ContainsKey("two"));
    }

    [TestMethod]
    public async Task Contents_WithTotal()
    {
        var timer = new TaskTimer();
        timer.Time("one");
        timer.Time("two");
        timer.AddTotal();

        Assert.AreEqual(3, timer.Timings.Count);

        Assert.IsTrue(timer.Timings.ContainsKey("one"));
        Assert.IsTrue(timer.Timings.ContainsKey("two"));
        Assert.IsTrue(timer.Timings.ContainsKey("total"));

        Assert.AreEqual(timer.Timings["total"], timer.Timings["one"] + timer.Timings["two"]);
    }

    [TestMethod]
    public async Task SimpleTimingsTest()
    {
        var timer = new TaskTimer();
        await Task.Delay(100);
        timer.Time("one");
        await Task.Delay(50);
        timer.Time("two");

        var ms = timer.GetTimingsInMs();
        Assert.AreEqual(100, ms["one"], 20);
        Assert.AreEqual(50, ms["two"], 20);
    }
}