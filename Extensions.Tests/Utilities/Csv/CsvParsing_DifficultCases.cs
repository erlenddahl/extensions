using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.erlenddahl.Extensions.Utilities.Csv;

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

        [TestMethod]
        public void NewLinesInsideTextColumns_InHeader()
        {
            var str = "colA;colB;\"colC\r\nYes\"\r\n0;1;\"yes this ; is one column\"\r\n2;3;4\r\n";
            var rows = FromString(str).ToArray();
            Assert.AreEqual(2, rows.Length);

            foreach (var row in rows)
            {
                CollectionAssert.AreEqual(new[] { "colA", "colB", "colC\r\nYes" }, row.Headers);
                Assert.AreEqual(3, row.Raw.Length);
            }

            Assert.AreEqual("0", rows[0][0]);
            Assert.AreEqual("1", rows[0][1]);
            Assert.AreEqual("yes this ; is one column", rows[0][2]);

            Assert.AreEqual("2", rows[1][0]);
            Assert.AreEqual("3", rows[1][1]);
            Assert.AreEqual("4", rows[1][2]);
        }

        [TestMethod]
        public void HasNoHeader()
        {
            var str = "colA;colB;\"colC\r\nYes\"\r\n0;1;\"yes this ; is one column\"\r\n2;3;4\r\n";
            var rows = FromString(str, new CsvSettings(){HasHeaders = false}).ToArray();
            Assert.AreEqual(3, rows.Length);

            foreach (var row in rows)
            {
                Assert.AreEqual(3, row.Raw.Length);
            }

            Assert.AreEqual("colA", rows[0][0]);
            Assert.AreEqual("colB", rows[0][1]);
            Assert.AreEqual("colC\r\nYes", rows[0][2]);


            Assert.AreEqual("0", rows[1][0]);
            Assert.AreEqual("1", rows[1][1]);
            Assert.AreEqual("yes this ; is one column", rows[1][2]);

            Assert.AreEqual("2", rows[2][0]);
            Assert.AreEqual("3", rows[2][1]);
            Assert.AreEqual("4", rows[2][2]);
        }

        [TestMethod]
        public void NewLinesInsideTextColumns_InHeaderAndData_WithEmptyLineAtEnd()
        {
            var str = "colA;colB;\"colC\r\nYes\"\r\n0;1;\"yes this\r\nis one column\"\r\n2;3;4\r\n";
            var rows = FromString(str).ToArray();
            Assert.AreEqual(2, rows.Length);

            foreach (var row in rows)
            {
                CollectionAssert.AreEqual(new[] { "colA", "colB", "colC\r\nYes" }, row.Headers);
                Assert.AreEqual(3, row.Raw.Length);
            }

            Assert.AreEqual("0", rows[0][0]);
            Assert.AreEqual("1", rows[0][1]);
            Assert.AreEqual("yes this\r\nis one column", rows[0][2]);

            Assert.AreEqual("2", rows[1][0]);
            Assert.AreEqual("3", rows[1][1]);
            Assert.AreEqual("4", rows[1][2]);
        }

        [TestMethod]
        public void NewLinesInsideTextColumns_InHeader_WithoutEmptyLineAtEnd()
        {
            var str = "colA;colB;\"colC\r\nYes\"\r\n0;1;\"yes this ; is one column\"\r\n2;3;4";
            var rows = FromString(str).ToArray();
            Assert.AreEqual(2, rows.Length);

            foreach (var row in rows)
            {
                CollectionAssert.AreEqual(new[] { "colA", "colB", "colC\r\nYes" }, row.Headers);
                Assert.AreEqual(3, row.Raw.Length);
            }

            Assert.AreEqual("0", rows[0][0]);
            Assert.AreEqual("1", rows[0][1]);
            Assert.AreEqual("yes this ; is one column", rows[0][2]);

            Assert.AreEqual("2", rows[1][0]);
            Assert.AreEqual("3", rows[1][1]);
            Assert.AreEqual("4", rows[1][2]);
        }

        [TestMethod]
        public void ReallifeData_WithSpecialSymbols()
        {
            var str = @"79,""{196108,574308734,573668313,574308735,34658451,197674,573668310,2118743465,34654741,4998029064,4998029059,3235841628,289941221,2083954750,197676,4998010282,2117958460,197677,2117958574,618970409,197678,2031053475,2117958482,197679,198720,4995004301,198721}"",""{cycleway:both,no,highway,primary,lanes,2,lit,yes,maxspeed,""""30 mph"""",name,""""East End Road"""",ref,A504,sidewalk,both,sidewalk:both:surface,paving_stones,smoothness,good,surface,asphalt}""
92,""{442734,442735,308149691,442736,4665821726,442737,4665821727,442738,442739,2429178447,442740}"",""{highway,residential,lit,yes,maxspeed,30,maxweight,3.5,maxweight:destination,none,name,Eigenheimstraße,postal_code,01217,source:maxspeed,DE:zone30,surface,asphalt,wikidata,Q72662287,zone:maxspeed,DE:30}""";

            var rows = FromString(str, new CsvSettings() { HasHeaders = false, Separator = ','}).ToArray();
            Assert.AreEqual(2, rows.Length);

            foreach (var row in rows)
            {
                Assert.AreEqual(3, row.Raw.Length);
            }
        }

        [TestMethod]
        public void ReallifeData_LotsOfQuotesAndNewlines()
        {
            var rows = FromFile(@"..\..\..\..\Data\UnitTests\CsvReader\lots_of_quotes_and_newlines.csv", new CsvSettings() { HasHeaders = false, Separator = ',' }).ToArray();
            Assert.AreEqual(25, rows.Length);

            foreach (var row in rows)
            {
                Assert.AreEqual(3, row.Raw.Length);
            }
        }
    }
}