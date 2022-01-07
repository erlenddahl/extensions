using System.Linq;
using Extensions.Utilities;
using Extensions.Utilities.Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Utilities.Csv
{
    [TestClass]
    public class RowSplitting_SemiColon: CsvReader
    {
        public RowSplitting_SemiColon() : base(';', '\"')
        {

        }

        [TestMethod]
        public void SimpleTest()
        {
            var csv = "a;b;c";
            var cols = SplitRow(csv).ToArray();
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, cols);
        }

        [TestMethod]
        public void No_Trimming()
        {
            var csv = " a;b ; c ";
            var cols = SplitRow(csv).ToArray();
            CollectionAssert.AreEqual(new[] { " a", "b ", " c " }, cols);
        }

        [TestMethod]
        public void Quotes()
        {
            var csv = " a;\"b \"; c ";
            var cols = SplitRow(csv).ToArray();
            CollectionAssert.AreEqual(new[] { " a", "b ", " c " }, cols);
        }

        [TestMethod]
        public void Quotes_With_Separator()
        {
            var csv = " a;\"b; \"; c ";
            var cols = SplitRow(csv).ToArray();
            CollectionAssert.AreEqual(new[] { " a", "b; ", " c " }, cols);
        }

        [TestMethod]
        public void EmptyColumn_Middle()
        {
            var csv = "a;;c";
            var cols = SplitRow(csv).ToArray();
            CollectionAssert.AreEqual(new[] { "a", "", "c" }, cols);
        }

        [TestMethod]
        public void EmptyColumn_Start()
        {
            var csv = ";b;c";
            var cols = SplitRow(csv).ToArray();
            CollectionAssert.AreEqual(new[] { "", "b", "c" }, cols);
        }

        [TestMethod]
        public void EmptyColumn_End()
        {
            var csv = "a;b;";
            var cols = SplitRow(csv).ToArray();
            CollectionAssert.AreEqual(new[] { "a", "b", "" }, cols);
        }
    }
}