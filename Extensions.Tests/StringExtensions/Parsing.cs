using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.StringExtensions;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class Parsing
    {

        [TestMethod]
        public void ToDoubleTests()
        {
            Assert.AreEqual(124.11, "124.11".ToDouble('.'), 0.0005);
            Assert.AreEqual(124.11, "124,11".ToDouble(','), 0.0005);

            Assert.AreEqual(0, "0.000".ToDouble('.'), 0.0005);
            Assert.AreEqual(0, "0,000".ToDouble(','), 0.0005);

            Assert.AreEqual(0, "0".ToDouble('.'), 0.0005);
            Assert.AreEqual(0, "0".ToDouble(','), 0.0005);

            Assert.AreEqual(-124.11, "-124.11".ToDouble('.'), 0.0005);
            Assert.AreEqual(-124.11, "-124,11".ToDouble(','), 0.0005);

            Assert.AreEqual(-5, "-as1".ToDouble('.', -5), 0.0005);
            Assert.AreEqual(-5, "-1fa1".ToDouble(',', -5), 0.0005);

            try
            {
                "-1fa1".ToDouble(',');
                Assert.Fail();
            }
            catch { }

            try
            {
                "-as1".ToDouble('.');
                Assert.Fail();
            }
            catch { }
        }

        [TestMethod]
        public void ToIntTests()
        {
            Assert.AreEqual(124, "124".ToInt());
            Assert.AreEqual(-124, "-124".ToInt());
            Assert.AreEqual(0, "0".ToInt());
            Assert.AreEqual(-5, "-1fsae24".ToInt(-5));
            
            try
            {
                "-1fa1".ToInt();
                Assert.Fail();
            }
            catch { }

            try
            {
                "-as1".ToInt();
                Assert.Fail();
            }
            catch { }
        }
    }
}