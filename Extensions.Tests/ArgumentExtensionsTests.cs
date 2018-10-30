using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests
{
    [TestClass]
    public class ArgumentExtensionsTests
    {
        [TestMethod]
        public void HasSwitchTests()
        {
            var args = new[] { "-alpha", "-beta", "-c" };
            Assert.AreEqual(true, args.HasSwitch("-alpha"));
            Assert.AreEqual(true, args.HasSwitch("-beta"));
            Assert.AreEqual(true, args.HasSwitch("-c"));
            Assert.AreEqual(false, args.HasSwitch("-coin"));
            Assert.AreEqual(false, args.HasSwitch("-is"));
            Assert.AreEqual(false, args.HasSwitch("-ice"));
        }

        [TestMethod]
        public void GetArgumentTests()
        {
            var args = new[] { "-alpha", "", "-beta", "hei", "-c", "hallo" };
            Assert.AreEqual("", args.GetArgument("-alpha"));
            Assert.AreEqual("", args.GetArgument("-beta"));
            Assert.AreEqual("", args.GetArgument("-c"));
            Assert.AreEqual("", args.GetArgument("-coin"));
            Assert.AreEqual("", args.GetArgument("-is"));
            Assert.AreEqual("", args.GetArgument("-ice"));
        }
    }
}
