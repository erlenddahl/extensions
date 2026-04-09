using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.StringExtensions;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class Csv
    {

        [TestMethod]
        public void ChangeSeparatorTests()
        {
            Assert.AreEqual("a;b;c", "a,b,c".ChangeSeparator(',', ';'));
            Assert.AreEqual("a;b,d;c", "a,\"b,d\",c".ChangeSeparator(',', ';'));
        }
    }
}