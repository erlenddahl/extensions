using Extensions.StringExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests.StringExtensions
{
    [TestClass]
    public class Cleaning
    {

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
        public void StripSpecialCharactersTests()
        {
            Assert.AreEqual("", "".StripSpecialCharacters());
            Assert.AreEqual("This is a normal, sentence!? This too.", "This is a normal, sentence!? This too.".StripSpecialCharacters());
            Assert.AreEqual("'Double quotes' are safified.", "\"Double quotes\" are safified.".StripSpecialCharacters());
            Assert.AreEqual(" ", " ".StripSpecialCharacters());
            Assert.AreEqual("", "\r\n\t".StripSpecialCharacters());
            Assert.AreEqual("a.,.a¤%#a;:_a.a", "a.,.a¤%#a;:_a.a".StripSpecialCharacters());
            Assert.AreEqual("    HR-0101   ", "    HR-0101   ".StripSpecialCharacters());
        }
    }
}