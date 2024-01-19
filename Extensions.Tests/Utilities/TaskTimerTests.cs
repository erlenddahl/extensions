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

    [TestMethod]
    public async Task WrapperTest()
    {
        var timer = new TaskTimer();
        await Task.Delay(100);
        timer.Time("one");
        await Task.Delay(50);
        timer.Time("two");

        var wrapper = TaskTimer.Wrap(timer, "wrapper.");
        await Task.Delay(150);
        wrapper.Time("three");

        Assert.AreEqual(3, timer.Timings.Count);
        Assert.AreEqual(1, wrapper.Timings.Count);

        var ms = timer.GetTimingsInMs();
        var wms = wrapper.GetTimingsInMs();
        Assert.AreEqual(100, ms["one"], 20);
        Assert.AreEqual(50, ms["two"], 20);
        Assert.AreEqual(150, ms["wrapper.three"], 20);
        Assert.AreEqual(150, wms["wrapper.three"], 20);

        // When repeating, the new wrapper should only have three once, but the inner
        // timer should now have it twice.
        wrapper = TaskTimer.Wrap(timer, "wrapper.");
        await Task.Delay(150);
        wrapper.Time("three");

        Assert.AreEqual(3, timer.Timings.Count);
        Assert.AreEqual(1, wrapper.Timings.Count);

        ms = timer.GetTimingsInMs();
        wms = wrapper.GetTimingsInMs();
        Assert.AreEqual(100, ms["one"], 20);
        Assert.AreEqual(50, ms["two"], 20);
        Assert.AreEqual(300, ms["wrapper.three"], 20);
        Assert.AreEqual(150, wms["wrapper.three"], 20);
    }

    [TestMethod]
    public async Task Wrapper_NoDoubleCounting()
    {
        var timer = new TaskTimer();
        await Task.Delay(100);
        timer.Time("one");
        await Task.Delay(50);
        timer.Time("two");

        var wrapper = TaskTimer.Wrap(timer, "wrapper.");
        await Task.Delay(150);
        wrapper.Time("three");

        timer.Time("four");

        Assert.AreEqual(4, timer.Timings.Count);
        Assert.AreEqual(1, wrapper.Timings.Count);

        var ms = timer.GetTimingsInMs();
        var wms = wrapper.GetTimingsInMs();
        Assert.AreEqual(100, ms["one"], 20);
        Assert.AreEqual(50, ms["two"], 20);
        Assert.AreEqual(150, ms["wrapper.three"], 20);
        Assert.AreEqual(150, wms["wrapper.three"], 20);
        Assert.AreEqual(0, ms["four"], 20);
    }
}