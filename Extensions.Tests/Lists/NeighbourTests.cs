using System;
using System.Collections.Generic;
using System.Linq;
using Extensions.Lists;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extensions.Tests
{
    [TestClass]
    public class NeighbourTests
    {
        [TestMethod]
        public void MatchingNeighbour_Normal()
        {
            var list = new int?[] {1, 2, 3, 4, null, null, null, 5, 6, 7, null, 8, null, null, null, null, null, null, 0, null, null, null, null};

            var match = list.MatchingNeighbours(0, p => p != null).ToList();
            Assert.AreEqual(1, match.Count);
            Assert.AreEqual(1, match[0].Distance);
            Assert.AreEqual(2, match[0].Neighbour);

            match = list.MatchingNeighbours(1, p => p != null).ToList();
            Assert.AreEqual(2, match.Count);
            Assert.AreEqual(1, match[0].Distance);
            Assert.AreEqual(3, match[0].Neighbour);
            Assert.AreEqual(1, match[1].Distance);
            Assert.AreEqual(1, match[1].Neighbour);

            match = list.MatchingNeighbours(3, p => p != null).ToList();
            Assert.AreEqual(2, match.Count);
            Assert.AreEqual(1, match[0].Distance);
            Assert.AreEqual(3, match[0].Neighbour);
            Assert.AreEqual(4, match[1].Distance);
            Assert.AreEqual(5, match[1].Neighbour);

            match = list.MatchingNeighbours(4, p => p != null).ToList();
            Assert.AreEqual(2, match.Count);
            Assert.AreEqual(1, match[0].Distance);
            Assert.AreEqual(4, match[0].Neighbour);
            Assert.AreEqual(3, match[1].Distance);
            Assert.AreEqual(5, match[1].Neighbour);

            match = list.MatchingNeighbours(5, p => p != null).ToList();
            Assert.AreEqual(2, match.Count);
            Assert.AreEqual(2, match[0].Distance);
            Assert.AreEqual(5, match[0].Neighbour);
            Assert.AreEqual(2, match[1].Distance);
            Assert.AreEqual(4, match[1].Neighbour);

            match = list.MatchingNeighbours(17, p => p != null).ToList();
            Assert.AreEqual(2, match.Count);
            Assert.AreEqual(1, match[0].Distance);
            Assert.AreEqual(0, match[0].Neighbour);
            Assert.AreEqual(6, match[1].Distance);
            Assert.AreEqual(8, match[1].Neighbour);

            match = list.MatchingNeighbours(18, p => p != null).ToList();
            Assert.AreEqual(1, match.Count);
            Assert.AreEqual(7, match[0].Distance);
            Assert.AreEqual(8, match[0].Neighbour);

            for (var i = 19; i < 23; i++)
            {
                match = list.MatchingNeighbours(i, p => p != null).ToList();
                Assert.AreEqual(1, match.Count);
                Assert.AreEqual(i - 18, match[0].Distance);
                Assert.AreEqual(0, match[0].Neighbour);
            }
        }

        [TestMethod]
        public void MatchingNeighbour_Outside_Bounds()
        {
            var list = new int?[] {1, 2, 3, 4, null, null, null, 5, 6, 7, null, 8, null, null, null, null, null, null, 0, null, null, null, null};

            for (var i = -10; i < 0; i++)
            {
                var match = list.MatchingNeighbours(i, p => p != null).ToList();
                Assert.AreEqual(0, match.Count);
            }

            for (var i = 23; i < 40; i++)
            {
                var match = list.MatchingNeighbours(i, p => p != null).ToList();
                Assert.AreEqual(0, match.Count);
            }
        }

        [TestMethod]
        public void GetNeightbours()
        {
            var arr = "A B C D E F G H I J K L".Split(' ');
            Assert.AreEqual("ABC",     string.Join("", arr.GetNeighbours(0, 4, 2)));
            Assert.AreEqual("ABCD",    string.Join("", arr.GetNeighbours(1, 4, 2)));
            Assert.AreEqual("ABCDE",   string.Join("", arr.GetNeighbours(2, 4, 2)));
            Assert.AreEqual("ABCDEF",  string.Join("", arr.GetNeighbours(3, 4, 2)));
            Assert.AreEqual("ABCDEFG", string.Join("", arr.GetNeighbours(4, 4, 2)));
            Assert.AreEqual("BCDEFGH", string.Join("", arr.GetNeighbours(5, 4, 2)));
            Assert.AreEqual("CDEFGHI", string.Join("", arr.GetNeighbours(6, 4, 2)));
            Assert.AreEqual("DEFGHIJ", string.Join("", arr.GetNeighbours(7, 4, 2)));
            Assert.AreEqual("EFGHIJK", string.Join("", arr.GetNeighbours(8, 4, 2)));
            Assert.AreEqual("FGHIJKL", string.Join("", arr.GetNeighbours(9, 4, 2)));
        }

        [TestMethod]
        public void GetNeightboursAll()
        {
            var arr = "A B C D E F G H I J K L".Split(' ').GetNeighbours(4, 2).ToList();
            Assert.AreEqual(12, arr.Count);

            Assert.AreEqual("ABC", string.Join("", arr[0]));
            Assert.AreEqual("ABCD", string.Join("", arr[1]));
            Assert.AreEqual("ABCDE", string.Join("", arr[2]));
            Assert.AreEqual("ABCDEF", string.Join("", arr[3]));
            Assert.AreEqual("ABCDEFG", string.Join("", arr[4]));
            Assert.AreEqual("BCDEFGH", string.Join("", arr[5]));
            Assert.AreEqual("CDEFGHI", string.Join("", arr[6]));
            Assert.AreEqual("DEFGHIJ", string.Join("", arr[7]));
            Assert.AreEqual("EFGHIJK", string.Join("", arr[8]));
            Assert.AreEqual("FGHIJKL", string.Join("", arr[9]));
            Assert.AreEqual("GHIJKL", string.Join("", arr[10]));
            Assert.AreEqual("HIJKL", string.Join("", arr[11]));
        }

        [TestMethod]
        public void GetNeightboursAllWithSelector()
        {
            var arr = "A B C D E F G H I J K L".Split(' ').GetNeighbours(4, 2, (i, p) => i + "_" + string.Join("", p)).ToList();
            Assert.AreEqual(12, arr.Count);

            Assert.AreEqual("0_ABC",     arr[0]);
            Assert.AreEqual("1_ABCD",    arr[1]);
            Assert.AreEqual("2_ABCDE",   arr[2]);
            Assert.AreEqual("3_ABCDEF",  arr[3]);
            Assert.AreEqual("4_ABCDEFG", arr[4]);
            Assert.AreEqual("5_BCDEFGH", arr[5]);
            Assert.AreEqual("6_CDEFGHI", arr[6]);
            Assert.AreEqual("7_DEFGHIJ", arr[7]);
            Assert.AreEqual("8_EFGHIJK", arr[8]);
            Assert.AreEqual("9_FGHIJKL", arr[9]);
            Assert.AreEqual("10_GHIJKL",  arr[10]);
            Assert.AreEqual("11_HIJKL",   arr[11]);
        }
    }
}
