using System.Linq;
using Extensions.Utilities.Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.Utilities.Csv
{
    [TestClass]
    public class CsvParsing_DifficultCases:CsvReader{
        [TestMethod]
        public void EscapedQuotes()
        {
            var str = "541254;4326523;\"fsdagggg\";2013-09-27 08:03:21+00:00;\"\\\"Bla, bla, bla…\";da;\"Instagram\";e42ed02b50d62e29;POINT (10.7431132 59.91449933);\"Description text.\";\"Oslo, Norway\";5512;1565;521351;1112;\"Oslo, Norge\";\"\";POLYGON (())";
            var cols = SplitRow(str).ToArray();
            Assert.AreEqual(18, cols.Length);
        }

        [TestMethod]
        public void SlashBeforeQuote()
        {
            var str = "5126126146;6126622;\"username\";2013-09-27 07:48:43+00:00;\"@someofmybest ok. I keep abrogating or limiting my word usage and I bore myself. =\\\\\";en;\"Tweetbot for Mac\";gfasghasgui;;\"Blah, blah\";\"he-him\";6111;233;5123561;51;\"Trondheim, Norge\";\"\";POLYGON (())";
            var cols = SplitRow(str).ToArray();
            Assert.AreEqual(18, cols.Length);
        }
    }
}