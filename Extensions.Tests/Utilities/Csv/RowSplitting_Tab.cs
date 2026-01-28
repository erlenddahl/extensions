using System.Linq;
using Extensions.Utilities;
using Extensions.Utilities.Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Utilities.Csv
{
    [TestClass]
    public class RowSplitting_Tab: CsvReader
    {
        public RowSplitting_Tab() : base(new CsvSettings() { Separator = '\t' })
        {

        }

        [TestMethod]
        public void SimpleTest()
        {
            var csv = "a\tb\tc";
            var cols = SplitRow(csv).ToArray();
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, cols);
        }

        [TestMethod]
        public void No_Trimming()
        {
            var csv = " a\tb \t c ";
            var cols = SplitRow(csv).ToArray();
            CollectionAssert.AreEqual(new[] { " a", "b ", " c " }, cols);
        }

        [TestMethod]
        public void Quotes()
        {
            var csv = " a\t\"b \"\t c ";
            var cols = SplitRow(csv).ToArray();
            CollectionAssert.AreEqual(new[] { " a", "b ", " c " }, cols);
        }

        [TestMethod]
        public void Quotes_With_Separator()
        {
            var csv = " a\t\"b\t \"\t c ";
            var cols = SplitRow(csv).ToArray();
            CollectionAssert.AreEqual(new[] { " a", "b\t ", " c " }, cols);
        }

        [TestMethod]
        public void EmptyColumn_Middle()
        {
            var csv = "a\t\tc";
            var cols = SplitRow(csv).ToArray();
            CollectionAssert.AreEqual(new[] { "a", "", "c" }, cols);
        }

        [TestMethod]
        public void EmptyColumn_Start()
        {
            var csv = "\tb\tc";
            var cols = SplitRow(csv).ToArray();
            CollectionAssert.AreEqual(new[] { "", "b", "c" }, cols);
        }

        [TestMethod]
        public void EmptyColumn_End()
        {
            var csv = "a\tb\t";
            var cols = SplitRow(csv).ToArray();
            CollectionAssert.AreEqual(new[] { "a", "b", "" }, cols);
        }
    }
}