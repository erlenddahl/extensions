using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests
{
    [TestClass]
    public class StringExtensions
    {
        [TestMethod]
        public void KeepBetweenTests()
        {
            try
            {
                "123=?erlend?=321".KeepBetween("µ", "<cat>");
                Assert.Fail();
            }
            catch (InvalidDataException ide) { }
            try
            {
                "123=?erlend?=321".KeepBetween("?=", "=?");
                Assert.Fail();
            }
            catch (InvalidDataException ide) { }
            try
            {
                "12µ3=?erlend?=321".KeepBetween("µ", "<cat>");
                Assert.Fail();
            }
            catch (InvalidDataException ide) { }
            try
            {
                "123=?erlend?=<cat>321".KeepBetween("µ", "<cat>");
                Assert.Fail();
            }
            catch (InvalidDataException ide) { }

            Assert.AreEqual("erlend", "123=?erlend?=321".KeepBetween("=?", "?="));
            Assert.AreEqual("=?erlend?=", "123=?erlend?=321".KeepBetween("=?", "?=", false));
            Assert.AreEqual("erlend", "=?erlend?=".KeepBetween("=?", "?="));
            Assert.AreEqual("=?erlend?=", "=?erlend?=".KeepBetween("=?", "?=", false));
            Assert.AreEqual("erlend", "?=xx?=xsa=?erlend?=da?=ffas?=".KeepBetween("=?", "?="));
            Assert.AreEqual("=?erlend?=", "?=xx?=xsa=?erlend?=da?=ffas?=".KeepBetween("=?", "?=", false));
        }

        [TestMethod]
        public void KeepBeforeTests()
        {
            Assert.AreEqual("", "".KeepBefore("<cat"));
            Assert.AreEqual("Someting cat hello", "Someting cat hello".KeepBefore("<cat"));
            Assert.AreEqual("Someting ", "Someting <cat> hello".KeepBefore("<cat"));
            Assert.AreEqual("", "Someting <cat> hello".KeepBefore("So"));
        }

        [TestMethod]
        public void MakeSafeForFilenameTests()
        {
            Assert.AreEqual("hei", "hei".MakeSafeForFilename());
            Assert.AreEqual("", "".MakeSafeForFilename());
            Assert.AreEqual("h--ei--", "h?:ei?:".MakeSafeForFilename("-"));
            Assert.AreEqual("hei", "h?:ei?:".MakeSafeForFilename());
            Assert.AreEqual("hei", "h**///\\\\?:e<>i?:".MakeSafeForFilename());
        }

        [TestMethod]
        public void KeepAfterTests()
        {
            Assert.AreEqual("", "".KeepAfter("<cat"));
            Assert.AreEqual("Someting cat hello", "Someting cat hello".KeepAfter("<cat"));
            Assert.AreEqual("<cat> hello", "Someting <cat> hello".KeepAfter("<cat>"));
            Assert.AreEqual("Someting <cat> hello", "Someting <cat> hello".KeepAfter("So"));

            Assert.AreEqual("Someting cat hello", "Someting cat hello".KeepAfter("<cat", false));
            Assert.AreEqual(" hello", "Someting <cat> hello".KeepAfter("<cat>", false));
            Assert.AreEqual("meting <cat> hello", "Someting <cat> hello".KeepAfter("So", false));
        }

        [TestMethod]
        public void RepeatTests()
        {
            Assert.AreEqual("", "".Repeat(0));
            Assert.AreEqual("", "fgaegaew".Repeat(0));
            Assert.AreEqual("", "".Repeat(100));
            Assert.AreEqual("aaaaa", "a".Repeat(5));
            Assert.AreEqual("aaaaaaaaaa", "aa".Repeat(5));
            Assert.AreEqual("aaaaaaaaaa".Repeat(2), "aa".Repeat(10));
            Assert.AreEqual(".-€.-€.-€", ".-€".Repeat(3));
        }

        [TestMethod]
        public void BrTests()
        {
            Assert.AreEqual(Environment.NewLine, "".Br());
            Assert.AreEqual(Environment.NewLine, "".Br(1));
            Assert.AreEqual(Environment.NewLine + Environment.NewLine, "".Br(2));
            Assert.AreEqual("hei" + Environment.NewLine, "hei".Br());
            Assert.AreEqual("hei" + Environment.NewLine, "hei".Br(1));
            Assert.AreEqual("hei" + Environment.NewLine + Environment.NewLine, "hei".Br(2));
        }

        [TestMethod]
        public void CleanTests()
        {
            Assert.AreEqual("", "".Clean("ABC"));
            Assert.AreEqual("", ".,;:;:_\"(/¤%)(!%".Clean("ABC"));
            Assert.AreEqual("aaaaa", "a.,.a¤%#a;:_a.a".Clean("abc"));
            Assert.AreEqual("aaaaa", "a.,.a¤%#a;:_a.a".Clean("ABc", true));
            Assert.AreEqual("", "a.,.a¤%#a;:_a.a".Clean("ABC"));
            Assert.AreEqual("ISA172", "ISA-172".Clean("ABCDEFGHIS01234567890"));
            Assert.AreEqual("HR20101", "    HR-20101   ".Clean("ABCDEFGHIRS01234567890"));

            Assert.AreEqual("aaaaa", "a.,.a¤%#a;:_a.a".Clean("A", true));
            Assert.AreEqual("", "a.,.a¤%#a;:_a.a".Clean("A", false));
        }

        [TestMethod]
        public void CleanAlphaNumericTests()
        {
            Assert.AreEqual("", "".CleanAlphaNumeric());
            Assert.AreEqual("", ".,;:;:_\"(/¤%)(!%".CleanAlphaNumeric());
            Assert.AreEqual("aaaaa", "a.,.a¤%#a;:_a.a".CleanAlphaNumeric());
            Assert.AreEqual("ISA172", "ISA-172".CleanAlphaNumeric());
            Assert.AreEqual("HR20101", "    HR-20101   ".CleanAlphaNumeric());
        }

        [TestMethod]
        public void CleanNumericTests()
        {
            Assert.AreEqual("", "".CleanNumeric());
            Assert.AreEqual("", ".,;:;:_\"(/¤%)(!%".CleanNumeric());
            Assert.AreEqual("", "a.,.a¤%#a;:_a.a".CleanNumeric());
            Assert.AreEqual("", "".CleanNumeric(true));
            Assert.AreEqual("", ".,;:;:_\"(/¤%)(!%".CleanNumeric(true));
            Assert.AreEqual("", "a.,.a¤%#a;:_a.a".CleanNumeric(true));
            Assert.AreEqual("172", "ISA-172".CleanNumeric(false));
            Assert.AreEqual("172", "ISA-172".CleanNumeric(true));
            Assert.AreEqual("20101", "    HR-20101   ".CleanNumeric());
            Assert.AreEqual("0101", "    HR-0101   ".CleanNumeric(false));
            Assert.AreEqual("72", "ISA-072".CleanNumeric(true));
            Assert.AreEqual("101", "    HR-00101   ".CleanNumeric(true));
            Assert.AreEqual("0", "    HR-00000   ".CleanNumeric(true));
            Assert.AreEqual("0", "    HR-0   ".CleanNumeric(true));
        }

        [TestMethod]
        public void CleanNumericWithDecimalsTests()
        {
            Assert.AreEqual("", "".CleanNumeric());
            Assert.AreEqual(".,", ".,;:;:_\"(/¤%)(!%".CleanNumericWithDecimals());
            Assert.AreEqual(".,..", "a.,.a¤%#a;:_a.a".CleanNumericWithDecimals());
            Assert.AreEqual("", "".CleanNumericWithDecimals(true));
            Assert.AreEqual(".,", ".,;:;:_\"(/¤%)(!%".CleanNumericWithDecimals(true));
            Assert.AreEqual(".,..", "a.Z,.a¤%#a;:_a.a".CleanNumericWithDecimals(true));
            Assert.AreEqual("17.2", "ISA-17.2".CleanNumericWithDecimals(false));
            Assert.AreEqual("17,2", "ISA-17,2".CleanNumericWithDecimals(true));
            Assert.AreEqual("20.101", "    HR-20.101   ".CleanNumericWithDecimals());
            Assert.AreEqual("01,01", "    HR-01,01   ".CleanNumericWithDecimals(false));
            Assert.AreEqual("72", "ISA-072".CleanNumericWithDecimals(true));
            Assert.AreEqual("101", "    HR-00101   ".CleanNumericWithDecimals(true));
            Assert.AreEqual("0", "    HR-00000   ".CleanNumericWithDecimals(true));
            Assert.AreEqual("0", "    HR-0   ".CleanNumericWithDecimals(true));
        }

        [TestMethod]
        public void PadBeforeTests()
        {
            Assert.AreEqual("  ", "".PadBefore(2));
            Assert.AreEqual("    Erlend", "Erlend".PadBefore(10));
            Assert.AreEqual("!!!!!Heppa!!!!!", "Heppa!!!!!".PadBefore(15, '!'));
        }

        [TestMethod]
        public void PadAfterTests()
        {
            Assert.AreEqual("", "".PadAfter(0));
            Assert.AreEqual("  ", "".PadAfter(2));
            Assert.AreEqual("Erlend    ", "Erlend".PadAfter(10));
            Assert.AreEqual("Erlend Dahl", "Erlend Dahl".PadAfter(2));
            Assert.AreEqual("Heppa!!!!!", "Heppa".PadAfter(10, '!'));
        }

        [TestMethod]
        public void PadCenterTests()
        {
            Assert.AreEqual("", "".PadCenter(0));
            Assert.AreEqual(" ", "".PadCenter(1));
            Assert.AreEqual("  ", "".PadCenter(2));

            Assert.AreEqual("A", "A".PadCenter(0));
            Assert.AreEqual("A", "A".PadCenter(1));
            Assert.AreEqual("A ", "A".PadCenter(2));
            Assert.AreEqual(" A ", "A".PadCenter(3));

            Assert.AreEqual("A", "A".PadCenter(0,'='));
            Assert.AreEqual("===ABBA===", "ABBA".PadCenter(10, '='));
            Assert.AreEqual("=====ABBA======", "ABBA".PadCenter(15, '='));
        }

        [TestMethod]
        public void ReplaceFirstTests()
        {
            Assert.AreEqual("", "".ReplaceFirst("", ""));
            Assert.AreEqual("", "".ReplaceFirst("alfa", "bravo"));
            Assert.AreEqual("bravo", "alfa".ReplaceFirst("alfa", "bravo"));
            Assert.AreEqual("bravoalfa", "alfaalfa".ReplaceFirst("alfa", "bravo"));
            Assert.AreEqual("baa", "aaa".ReplaceFirst("a", "b"));
            Assert.AreEqual("bba", "aaa".ReplaceFirst("a", "b").ReplaceFirst("a", "b"));
        }

        [TestMethod]
        public void CapitalizeFirstTests()
        {
            Assert.AreEqual("",       "".CapitalizeFirst());
            Assert.AreEqual("John",   "John".CapitalizeFirst());
            Assert.AreEqual("John",   "john".CapitalizeFirst());
            Assert.AreEqual("A",      "a".CapitalizeFirst());
            Assert.AreEqual("ABC",    "ABC".CapitalizeFirst());
            Assert.AreEqual("ABC",    "aBC".CapitalizeFirst());
        }

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

        [TestMethod]
        public void ChangeExtensionTests()
        {
            Assert.AreEqual("", "".ChangeExtension(".ldb"));
            Assert.AreEqual(null, ((string)null).ChangeExtension(".ldb"));

            Assert.AreEqual("C:\\test.ldb", "C:\\test".ChangeExtension(".ldb"));

            Assert.AreEqual("C:\\test.ldb", "C:\\test.mdb".ChangeExtension(".ldb"));
            Assert.AreEqual("C:\\te.s.t.ldb", "C:\\te.s.t.mdb".ChangeExtension(".ldb"));
            Assert.AreEqual("C:\\te\\s\\t.ldb", "C:\\te\\s\\t.mdb".ChangeExtension(".ldb"));
        }

        [TestMethod]
        public void RemoveExtensionTests()
        {
            Assert.AreEqual("", "".RemoveExtension());
            Assert.AreEqual(null, ((string)null).RemoveExtension());

            Assert.AreEqual("C:\\test", "C:\\test".RemoveExtension());

            Assert.AreEqual("C:\\test", "C:\\test.mdb".RemoveExtension());
            Assert.AreEqual("C:\\te.s.t", "C:\\te.s.t.mdb".RemoveExtension());
            Assert.AreEqual("C:\\te.faef\\fact", "C:\\te.faef\\fact.mdb".RemoveExtension());
            Assert.AreEqual("C:\\te\\s\\t", "C:\\te\\s\\t.mdb".RemoveExtension());
        }

        [TestMethod]
        public void IndicesOfTest()
        {
            var indices = "abcdabcdeafghijA".IndicesOf("a").ToArray();
            Assert.AreEqual(3, indices.Length);
            Assert.AreEqual(0, indices[0]);
            Assert.AreEqual(4, indices[1]);
            Assert.AreEqual(9, indices[2]);

            indices = "".IndicesOf("a").ToArray();
            Assert.AreEqual(0, indices.Length);

            indices = "aaa".IndicesOf("a").ToArray();
            Assert.AreEqual(3, indices.Length);
            Assert.AreEqual(0, indices[0]);
            Assert.AreEqual(1, indices[1]);
            Assert.AreEqual(2, indices[2]);
        }

        [TestMethod]
        public void IndexOfBefore()
        {
            var i = "abcdabcdeafghijA".IndexOfBefore("a", 0);
            Assert.AreEqual(-1, i);

            i = "abcdabcdeafghijA".IndexOfBefore("a", 1);
            Assert.AreEqual(0, i);

            i = "abcdabcdeafghijA".IndexOfBefore("a", 2);
            Assert.AreEqual(0, i);

            i = "abcdabcdeafghijA".IndexOfBefore("a", 3);
            Assert.AreEqual(0, i);

            i = "abcdabcdeafghijA".IndexOfBefore("a", 4);
            Assert.AreEqual(0, i);

            i = "abcdabcdeafghijA".IndexOfBefore("a", 5);
            Assert.AreEqual(4, i);

            i = "abcdabcdeafghijA".IndexOfBefore("a", 6);
            Assert.AreEqual(4, i);
        }

        [TestMethod]
        public void IndexOfAfter()
        {
            var i = "abcdabcdeafghijA".IndexOfAfter("a", 0);
            Assert.AreEqual(4, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 1);
            Assert.AreEqual(4, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 2);
            Assert.AreEqual(4, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 3);
            Assert.AreEqual(4, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 4);
            Assert.AreEqual(9, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 5);
            Assert.AreEqual(9, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 6);
            Assert.AreEqual(9, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 9);
            Assert.AreEqual(-1, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 10);
            Assert.AreEqual(-1, i);

            i = "abcdabcdeafghijA".IndexOfAfter("a", 11);
            Assert.AreEqual(-1, i);
        }

        [TestMethod]
        public void ReplaceSurroundingTests()
        {
            var i = "abcdabcdeafghijA".ReplaceSurrounding(5, "a", "c", "ABC");
            Assert.AreEqual("abcdABCdeafghijA", i);

            i = "abcdabcdeafghijA".ReplaceSurrounding(5, "c", "c", "");
            Assert.AreEqual("abdeafghijA", i);

            i = "abcdabcdeafghijA".ReplaceSurrounding(5, "EKKO", "c", "");
            Assert.AreEqual("abcdabcdeafghijA", i);

            i = "abcdabcdeafghijA".ReplaceSurrounding(5, "c", "PEKKO", "");
            Assert.AreEqual("abcdabcdeafghijA", i);
        }

        [TestMethod]
        public void RemoveEmptyLinesTests()
        {
            Assert.AreEqual("", "".RemoveEmptyLines());
            Assert.AreEqual("a", "a".RemoveEmptyLines());
            Assert.AreEqual("abcµ*", "abcµ*".RemoveEmptyLines());
            Assert.AreEqual("a", "a".Br().RemoveEmptyLines());
            Assert.AreEqual("a".Br() + "b", ("a".Br(3) + Environment.NewLine + "\t   " + Environment.NewLine + "b").RemoveEmptyLines());

            var answer = "Vind- og friksjonsvarsling Dovre             4.0".Br() + "All                                         44.0";
            var test = @"Vind- og friksjonsvarsling Dovre             4.0

All                                         44.0".RemoveEmptyLines();
            Assert.AreEqual(answer, test);
        }

        [TestMethod]
        public void ToCamelCaseTests()
        {
            Assert.AreEqual("hei", "Hei".ToCamelCase());
            Assert.AreEqual("heiOgHallo", "Hei og hallo".ToCamelCase());
            Assert.AreEqual("hei", "262Hei,.,,-.,".ToCamelCase());
            Assert.AreEqual("hybridElectricGasoline", "Hybrid, electric/gasoline".ToCamelCase());
        }

        [TestMethod]
        public void CleanLettersTests()
        {
            Assert.AreEqual("", "".CleanLetters());
            Assert.AreEqual("", ".,;:;:_\"(/¤%)(!%".CleanLetters());
            Assert.AreEqual("aaaaa", "a.,.a¤%#a;:_a.a".CleanLetters());
            Assert.AreEqual("ISA", "ISA-172".CleanLetters());
            Assert.AreEqual("HR", "    HR-20101   ".CleanLetters());
            Assert.AreEqual("HR", "    HR-0101   ".CleanLetters());
            Assert.AreEqual("HR", "    HR-00101   ".CleanLetters());
        }

        [TestMethod]
        public void RemoveTrailingZeroesTests()
        {
            Assert.AreEqual("0", "".RemoveTrailingZeroes());
            Assert.AreEqual("0", ((string)null).RemoveTrailingZeroes());
            Assert.AreEqual("0", "0.".RemoveTrailingZeroes());
            Assert.AreEqual("0", ".0".RemoveTrailingZeroes());
            Assert.AreEqual("0", ".000".RemoveTrailingZeroes());
            Assert.AreEqual("0", "0.0000".RemoveTrailingZeroes());
            Assert.AreEqual("0.1", "0.1000000".RemoveTrailingZeroes());
            Assert.AreEqual("0.1324354", "0.1324354".RemoveTrailingZeroes());
            Assert.AreEqual("0.1324354", "0.13243540".RemoveTrailingZeroes());
            Assert.AreEqual("0.1324354", "0.132435400".RemoveTrailingZeroes());
            Assert.AreEqual("0.1324354", "0.1324354000".RemoveTrailingZeroes());
            Assert.AreEqual("abcde", "abcde".RemoveTrailingZeroes());
            Assert.AreEqual("abcde", "abcde000000".RemoveTrailingZeroes());
        }

        [TestMethod]
        public void TruncatePathTests()
        {
            Assert.AreEqual(@"C:\folder\one\two\three\file.txt", @"C:\folder\one\two\three\file.txt".TruncatePath(500));
            Assert.AreEqual(@"C:\folder\one\two\three\file.txt", @"C:\folder\one\two\three\file.txt".TruncatePath(35));
            Assert.AreEqual(@"C:\folder\one\...\file.txt", @"C:\folder\one\two\three\file.txt".TruncatePath(27));
            Assert.AreEqual(@"C:\folder\...\file.txt", @"C:\folder\one\two\three\file.txt".TruncatePath(23));
            Assert.AreEqual(@"C:\...\file.txt", @"C:\folder\one\two\three\file.txt".TruncatePath(15));
            Assert.AreEqual(@"C:\...\file.txt", @"C:\folder\one\two\three\file.txt".TruncatePath(10));
            Assert.AreEqual(@"C:\...\file.txt", @"C:\folder\one\two\three\file.txt".TruncatePath(5));
        }

        [TestMethod]
        public void SubstringsOfLengthTests()
        {
            var abc = "abcdefghijklmnopqrstuvwxyz";
            var strs = abc.SubstringsOfLength(1, 1, 2, 3, 4, 5).ToArray();
            Assert.AreEqual("a", strs[0]);
            Assert.AreEqual("b", strs[1]);
            Assert.AreEqual("cd", strs[2]);
            Assert.AreEqual("efg", strs[3]);
            Assert.AreEqual("hijk", strs[4]);
            Assert.AreEqual("lmnop", strs[5]);

            strs = abc.SubstringsOfLength(10, 10).ToArray();
            Assert.AreEqual("abcdefghij", strs[0]);
            Assert.AreEqual("klmnopqrst", strs[1]);
        }

        [TestMethod]
        public void ChangeSeparatorTests()
        {
            Assert.AreEqual("a;b;c", "a,b,c".ChangeSeparator(',', ';'));
            Assert.AreEqual("a;b,d;c", "a,\"b,d\",c".ChangeSeparator(',', ';'));
        }
    }
}
