using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ByteUtilities.Tests
{
    [TestClass]
    public class ByteReaderTests
    {
        [TestMethod]
        public void TakeTest()
        {
            var tester = new ByteReaderTester(new byte[] { 0, 0, 1, 1, 0, 1, 54, 3, 6, 0, 1 });
            Assert.AreEqual(tester.ByteCount, 11);

            var take = tester.Take(0);
            Assert.AreEqual(tester.ByteCount, 11);
            Assert.AreEqual(take.Count(), 0);

            take = tester.Take(1);
            Assert.AreEqual(tester.ByteCount, 10);
            Assert.AreEqual(take.Count(), 1);

            take = tester.Take(3);
            Assert.AreEqual(tester.ByteCount, 7);
            Assert.AreEqual(take.Count(), 3);

            take = tester.Take(0);
            Assert.AreEqual(tester.ByteCount, 7);
            Assert.AreEqual(take.Count(), 0);

            take = tester.Take(4);
            Assert.AreEqual(tester.ByteCount, 3);
            Assert.AreEqual(take.Count(), 4);

            take = tester.Take(3);
            Assert.AreEqual(tester.ByteCount, 0);
            Assert.AreEqual(take.Count(), 3);

            take = tester.Take(0);
            Assert.AreEqual(tester.ByteCount, 0);
            Assert.AreEqual(take.Count(), 0);

            try
            {
                take = tester.Take(10);
                Assert.Fail();
            }
            catch (Exception ex)
            {

            }
        }

        [TestMethod]
        public void PeekTest()
        {
            var tester = new ByteReaderTester(new byte[] { 0, 0, 1, 1, 0, 1, 54, 3, 6, 0, 1 });
            Assert.AreEqual(tester.ByteCount, 11);
            for (var i = 0; i < 11; i++)
            {
                tester.Peek(i);
                Assert.AreEqual(tester.ByteCount, 11);
            }

            try
            {
                tester.Peek(15);
                Assert.Fail();
            }
            catch (Exception ex) { }
        }

        [TestMethod]
        public void TakeOneTest()
        {
            var bytes = new byte[] { 0, 0, 1, 1, 0, 1, 54, 3, 6, 0, 1 };
            var tester = new ByteReaderTester(bytes);

            for (var i = 0; i < bytes.Length; i++)
            {
                var take = tester.TakeOne();
                Assert.AreEqual(tester.ByteCount, 10 - i);
                Assert.AreEqual(take, bytes[i]);
            }

            try
            {
                tester.TakeOne();
                Assert.Fail();
            }
            catch (InvalidDataException ex)
            {
            }
        }

        [TestMethod]
        public void TakeNumberTest()
        {
            var bytes = new byte[] { 0, 0, 1, 1, 0, 1, 54, 3, 6, 0, 1 };
            var numbers = new[] { 16842752, 53870848 };
            var tester = new ByteReaderTester(bytes);

            var take = tester.TakeInt32();
            Assert.AreEqual(tester.ByteCount, 7);
            Assert.AreEqual(take, numbers[0]);

            take = tester.TakeInt32();
            Assert.AreEqual(tester.ByteCount, 3);
            Assert.AreEqual(take, numbers[1]);

            try
            {
                tester.TakeInt32();
                Assert.Fail();
            }
            catch (InvalidDataException ex)
            {
            }
        }

        [TestMethod]
        public void TakeNumbersTest()
        {
            var bytes = new byte[] { 0, 0, 1, 1, 0, 1, 54, 3, 6, 0, 1 };
            var numbers = new[] { 16842752, 53870848 };
            var tester = new ByteReaderTester(bytes);

            var take = tester.TakeInt32s(2);
            Assert.AreEqual(take[0], numbers[0]);
            Assert.AreEqual(take[1], numbers[1]);

            Assert.AreEqual(tester.ByteCount, 3);

            try
            {
                tester.TakeInt32();
                Assert.Fail();
            }
            catch (InvalidDataException ex)
            {
            }
        }

        [TestMethod]
        public void PeekNumberTest()
        {
            var bytes = new byte[] { 0, 0, 1, 1, 0, 1, 54, 3, 6, 0, 1 };
            var numbers = new[] { 16842752, 53870848 };
            var tester = new ByteReaderTester(bytes);

            var take = tester.PeekInt32();
            Assert.AreEqual(tester.ByteCount, 11);
            Assert.AreEqual(take, numbers[0]);

            take = tester.PeekInt32();
            Assert.AreEqual(tester.ByteCount, 11);
            Assert.AreEqual(take, numbers[0]);

            tester.TakeInt32(); //Remove 4 bytes

            take = tester.PeekInt32();
            Assert.AreEqual(tester.ByteCount, 7);
            Assert.AreEqual(take, numbers[1]);

            take = tester.PeekInt32();
            Assert.AreEqual(tester.ByteCount, 7);
            Assert.AreEqual(take, numbers[1]);

            tester.TakeInt32(); //Remove 4 bytes

            try
            {
                tester.PeekInt32();
                Assert.Fail();
            }
            catch (InvalidDataException ex)
            {
            }
        }

        [TestMethod]
        public void TakeRealTest()
        {
            var bytes = new byte[] { 82, 0, 1, 1, 0, 1, 54, 3, 223, 0, 1, 0, 0, 1, 1, 0, 1, 54 };
            var numbers = new[] { 3.4452760017795816E-293, 1.3961034711167646E-309 };
            var tester = new ByteReaderTester(bytes);

            var take = tester.TakeDouble();
            Assert.AreEqual(tester.ByteCount, 10);
            Assert.AreEqual(take, numbers[0]);

            take = tester.TakeDouble();
            Assert.AreEqual(tester.ByteCount, 2);
            Assert.AreEqual(take, numbers[1]);

            try
            {
                tester.TakeDouble();
                Assert.Fail();
            }
            catch (InvalidDataException ex)
            {
            }
        }

        [TestMethod]
        public void VerifyTest()
        {
            var bytes = new byte[] { 82, 0, 1, 1, 0, 1, 54, 3, 223, 0, 1, 0, 0, 1, 1, 0, 1, 54 };
            var tester = new ByteReaderTester(bytes);

            tester.Verify(new byte[] { 82 });
            Assert.AreEqual(17, tester.ByteCount);

            tester.Verify(new byte[] { });
            Assert.AreEqual(17, tester.ByteCount);

            tester.Verify(new byte[] { 0, 1, 1, 0, 1, 54 });
            Assert.AreEqual(11, tester.ByteCount);

            try
            {
                tester.Verify(new byte[] { 3, 223, 0, 1, 0, 0, 1, 1, 1 });
                Assert.Fail();
            }
            catch (InvalidDataException ex) { }
            Assert.AreEqual(11, tester.ByteCount);

            tester.Verify(new byte[] { 3, 223, 0, 1, 0, 0, 1, 1, 0 });
            Assert.AreEqual(2, tester.ByteCount);

            tester.Verify(new byte[] { });
            Assert.AreEqual(2, tester.ByteCount);

            try
            {
                tester.Verify(new byte[] { 1, 54, 3 });
                Assert.Fail();
            }
            catch (InvalidDataException ex) { }
            Assert.AreEqual(2, tester.ByteCount);

            tester.Verify(new byte[] { 1, 54 });
            Assert.AreEqual(0, tester.ByteCount);

            tester.Verify(new byte[] { });
            Assert.AreEqual(0, tester.ByteCount);

            try
            {
                tester.Verify(new byte[] { 1, 54, 3 });
                Assert.Fail();
            }
            catch (InvalidDataException ex) { }
            Assert.AreEqual(0, tester.ByteCount);
        }

        [TestMethod]
        public void TakeDynamicStringTest()
        {
            var bytes = new byte[]
            {
                9, 0, 0, 0, 72, 105, 103, 104, 119, 97, 121, 121, 121, 7, 0, 0, 0,
                (byte) 'R', (byte) 'o', (byte) 'a', (byte) 'd', (byte) 'w', (byte) 'a', (byte) 'y'
            };
            var tester = new ByteReaderTester(bytes);

            var take = tester.TakeDynamicString();
            Assert.AreEqual(11, tester.ByteCount);
            Assert.AreEqual("Highwayyy", take);

            take = tester.TakeDynamicString();
            Assert.AreEqual(0, tester.ByteCount);
            Assert.AreEqual("Roadway", take);

            try
            {
                tester.TakeDynamicString();
                Assert.Fail();
            }
            catch (InvalidDataException ide) { }
        }

        [TestMethod]
        public void PeekDynamicStringTest()
        {
            var bytes = new byte[]
            {
                9, 0, 0, 0, 72, 105, 103, 104, 119, 97, 121, 121, 121, 7, 0, 0, 0,
                (byte) 'R', (byte) 'o', (byte) 'a', (byte) 'd', (byte) 'w', (byte) 'a', (byte) 'y'
            };
            var tester = new ByteReaderTester(bytes);

            var take = tester.PeekDynamicString();
            Assert.AreEqual(24, tester.ByteCount);
            Assert.AreEqual("Highwayyy", take);

            take = tester.PeekDynamicString();
            Assert.AreEqual(24, tester.ByteCount);
            Assert.AreEqual("Highwayyy", take);

            tester.TakeDynamicString();

            take = tester.PeekDynamicString();
            Assert.AreEqual(11, tester.ByteCount);
            Assert.AreEqual("Roadway", take);

            take = tester.PeekDynamicString();
            Assert.AreEqual(11, tester.ByteCount);
            Assert.AreEqual("Roadway", take);

            tester.TakeDynamicString();

            try
            {
                tester.PeekDynamicString();
                Assert.Fail();
            }
            catch (InvalidDataException ide) { }
        }

        [TestMethod]
        public void PeekVerifyTest()
        {
            var bytes = new byte[] { 82, 0, 1, 1, 0, 1, 54, 3, 223, 0, 1, 0, 0, 1, 1, 0, 1, 54 };
            var tester = new ByteReaderTester(bytes);

            Assert.AreEqual(true, tester.PeekVerify(new byte[] { 82 }));
            Assert.AreEqual(18, tester.ByteCount);

            Assert.AreEqual(true, tester.PeekVerify(new byte[] { }));
            Assert.AreEqual(18, tester.ByteCount);

            Assert.AreEqual(true, tester.PeekVerify(new byte[] { 82, 0, 1, 1, 0, 1, 54 }));
            Assert.AreEqual(18, tester.ByteCount);

            Assert.AreEqual(false, tester.PeekVerify(new byte[] { 82, 0, 1, 0, 0, 1, 54 }));
            Assert.AreEqual(18, tester.ByteCount);

            Assert.AreEqual(false, tester.PeekVerify(new byte[] { 82, 0, 1, 0, 0, 1, 54, 0, 1, 0, 0, 1, 54, 0, 1, 0, 0, 1, 54, 0, 1, 0, 0, 1, 54, 0, 1, 0, 0, 1, 54 }));
            Assert.AreEqual(18, tester.ByteCount);
        }

        [TestMethod]
        public void TakeBoolTest()
        {
            var tester = new ByteReaderTester(new byte[] { 0, 0, 1, 1, 0, 1, 54, 3, 6, 0, 1 });
            Assert.AreEqual(false, tester.TakeBool());
            Assert.AreEqual(false, tester.TakeBool());
            Assert.AreEqual(true, tester.TakeBool());
            Assert.AreEqual(true, tester.TakeBool());
            Assert.AreEqual(false, tester.TakeBool());
            Assert.AreEqual(true, tester.TakeBool());

            try
            {
                tester.TakeBool();
                Assert.Fail("Should throw an exception!");
            }
            catch (InvalidDataException ex) { }

            try
            {
                tester.TakeBool();
                Assert.Fail("Should throw an exception!");
            }
            catch (InvalidDataException ex) { }

            try
            {
                tester.TakeBool();
                Assert.Fail("Should throw an exception!");
            }
            catch (InvalidDataException ex) { }

            Assert.AreEqual(false, tester.TakeBool());
            Assert.AreEqual(true, tester.TakeBool());
        }

        [TestMethod]
        public void SkipUntilTest()
        {
            var bytes = new byte[] { 82, 0, 1, 1, 0, 1, 54, 3, 223, 0, 1, 0, 0, 1, 1, 0, 1, 54 };
            var tester = new ByteReaderTester(bytes);

            CollectionAssert.AreEqual(new byte[] { }, tester.SkipUntil(new byte[] { 82 }, false).ToArray());
            CollectionAssert.AreEqual(bytes, tester.RemainingBytes);

            CollectionAssert.AreEqual(new byte[] { 82 }, tester.SkipUntil(new byte[] { 0, 1, 1 }, false).ToArray());
            CollectionAssert.AreEqual(bytes.Skip(1).ToArray(), tester.RemainingBytes);

            CollectionAssert.AreEqual(new byte[] { 0, 1, 1 }, tester.SkipUntil(new byte[] { 0, 1, 1 }, true).ToArray());
            CollectionAssert.AreEqual(bytes.Skip(4).ToArray(), tester.RemainingBytes);

            CollectionAssert.AreEqual(new byte[] { 0, 1, 54, 3, 223, 0, 1, 0, 0, 1, 1 }, tester.SkipUntil(new byte[] { 0, 1, 1 }, true).ToArray());
            CollectionAssert.AreEqual(bytes.Skip(15).ToArray(), tester.RemainingBytes);

            try
            {
                tester.SkipUntil(new byte[] { 0, 1, 1 }, true);
                Assert.Fail();
            }
            catch (InvalidDataException ex) { }
        }

        [TestMethod]
        public void TakeLongBoolTest()
        {
            var bytes = new byte[] { 255, 255, 0, 0, 254, 51 };
            var tester = new ByteReaderTester(bytes);

            Assert.AreEqual(true, tester.TakeLongBool());
            Assert.AreEqual(false, tester.TakeLongBool());

            try
            {
                tester.TakeLongBool();
                Assert.Fail();
            }
            catch (InvalidDataException ex) { }
        }
    }

    public class ByteReaderTester : ByteReader
    {
        public byte[] Bytes { get { return _bytes; } }
        public byte[] RemainingBytes { get { return _bytes.Skip(_position).ToArray(); } }

        public ByteReaderTester(string file)
            : base(file)
        {
        }

        public ByteReaderTester(byte[] bytes)
            : base(bytes)
        {
        }
    }
}