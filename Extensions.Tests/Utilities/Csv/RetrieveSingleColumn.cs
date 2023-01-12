using System;
using System.Linq;
using Extensions.Utilities;
using Extensions.Utilities.Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Utilities.Csv
{
    [TestClass]
    public class RetrieveSingleColumn : CsvReader
    {
        public RetrieveSingleColumn() : base(';', '\"')
        {

        }

        [TestMethod]
        public void SimpleTest()
        {
            var csv = "a;b;c;d;e;fgh;\"ij\";k;l";
            var cols = SplitRow(csv).ToArray();
            for (var i = 0; i < cols.Length; i++)
            {
                Assert.AreEqual(cols[i], SplitRowAndRetrieveSingleColumn(csv, i));
            }
        }

        [TestMethod]
        public void FailsBelowZero()
        {
            var csv = "a;b;c;d;e;fgh;\"ij\";k;l";
            var cols = SplitRow(csv).ToArray();
            try
            {
                SplitRowAndRetrieveSingleColumn(csv, -1);
                Assert.Fail();
            }
            catch (Exception ex)
            {

            }
        }

        [TestMethod]
        public void FailsAboveMax()
        {
            var csv = "a;b;c;d;e;fgh;\"ij\";k;l";
            var cols = SplitRow(csv).ToArray();
            try
            {
                SplitRowAndRetrieveSingleColumn(csv, 9);
                Assert.Fail();
            }
            catch (Exception ex)
            {

            }
        }
    }
}