using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteUtilities
{
    public class ByteReader
    {
        protected byte[] _bytes;
        protected int _position = 0;
        public int ByteCount { get { return _bytes.Length - _position; } }

        public ByteReader(string file)
        {
            _bytes = File.ReadAllBytes(file);
        }

        public ByteReader(byte[] bytes)
        {
            _bytes = bytes;
        }

        /// <summary>
        /// Will return the first n bytes from the stream and remove them from the stream.
        /// </summary>
        /// <param name="n">The number of bytes to return.</param>
        /// <returns>The n first bytes in the stream.</returns>
        public byte[] Take(int n)
        {
            var bytes = new byte[n];
            Array.Copy(_bytes, _position, bytes, 0, n);
            _position += n;
            return bytes;
        }

        /// <summary>
        /// Will return the first n bytes from the stream without removing them.
        /// </summary>
        /// <param name="n">The number of bytes to return.</param>
        /// <returns>The n first bytes in the stream.</returns>
        public IEnumerable<byte> Peek(int n)
        {
            var bytes = new byte[n];
            for (var i = 0; i < n; i++)
                bytes[i] = _bytes[i + _position];
            return bytes;
        }

        /// <summary>
        /// Will take a single byte from the byte stream.
        /// </summary>
        /// <returns>The first byte in the stream.</returns>
        public byte TakeOne()
        {
            if (ByteCount < 1) throw new InvalidDataException("There's nothing left in this byte stream. Please let erlend.dahl@sintef.no know.");
            return _bytes[_position++];
        }

        /// <summary>
        /// Will take the first four bytes of the byte stream, and convert them to an Int32.
        /// </summary>
        /// <returns>The number represented by the first four bytes in the stream.</returns>
        public int TakeInt32()
        {
            if (ByteCount < 4) throw new InvalidDataException("There are not enough bytes left to create a number. Please let erlend.dahl@sintef.no know.");
            var num = BitConverter.ToInt32(_bytes, _position);
            _position += 4;
            return num;
        }

        /// <summary>
        /// Will take the first four bytes of the byte stream N times, and convert them to Int32s.
        /// </summary>
        /// <returns>An array of the numbers represented by the N first four bytes in the stream.</returns>
        public int[] TakeInt32s(int count)
        {
            if (ByteCount < 4 * count) throw new InvalidDataException("There are not enough bytes left to create " + count + " Int32s. Please let erlend.dahl@sintef.no know.");
            var ints = new int[count];
            for (var i = 0; i < count; i++)
                ints[i] = TakeInt32();
            return ints;
        }

        /// <summary>
        /// Will return the number represented by the first four bytes in the byte stream, without removing it from the stream.
        /// </summary>
        /// <returns>The number represented by the first four bytes in the stream.</returns>
        public int PeekInt32()
        {
            if (ByteCount < 4) throw new InvalidDataException("There are not enough bytes left to create a number. Please let erlend.dahl@sintef.no know.");
            return BitConverter.ToInt32(_bytes, _position);
        }

        /// <summary>
        /// Will take the first eight bytes of the byte stream, and convert them to a Double.
        /// </summary>
        /// <returns>The number represented by the first eight bytes in the stream.</returns>
        public double TakeDouble()
        {
            if (ByteCount < 8) throw new InvalidDataException("There are not enough bytes left to create a double. Please let erlend.dahl@sintef.no know.");
            var num = BitConverter.ToDouble(_bytes, _position);
            _position += 8;
            return num;
        }

        /// <summary>
        /// Will take the first eight bytes of the byte stream N times, and convert them to Doubles.
        /// </summary>
        /// <returns>An array of the numbers represented by the N first eight bytes in the stream.</returns>
        public double[] TakeDoubles(int count)
        {
            if (ByteCount < 8 * count) throw new InvalidDataException("There are not enough bytes left to create " + count + " doubles. Please let erlend.dahl@sintef.no know.");
            var doubles = new double[count];
            for (var i = 0; i < count; i++)
                doubles[i] = TakeDouble();
            return doubles;
        }

        /// <summary>
        /// Will verify that the next n bytes are identical to the given bytes, and remove them. If they are not, this function will throw an exception.
        /// </summary>
        /// <param name="bs">The bytes to verify against.</param>
        public void Verify(byte[] bs)
        {
            if (bs.Length > ByteCount)
                throw new InvalidDataException("The given bytes did not match. Please let erlend.dahl@sintef.no know.");
            for (var i = 0; i < bs.Length; i++)
                if (_bytes[_position + i] != bs[i]) throw new InvalidDataException("The given bytes did not match. Please let erlend.dahl@sintef.no know.");
            _position += bs.Length;
        }

        /// <summary>
        /// Will read and return the string represented by the following 4-n bytes. In the CAT file, a string is stored as an int (the length of the string), 
        /// and then the string itself. This function will read (and remove) the int first, and then read (and remove) the following bytes representing the string.
        /// </summary>
        /// <returns>The string represented by the first 4-n bytes in the stream.</returns>
        public string TakeDynamicString()
        {
            var stringLength = PeekInt32();
            if (ByteCount < stringLength) throw new InvalidDataException("Not enough bytes left to take this string. Please let erlend.dahl@sintef.no know.");
            _position += 4;
            return System.Text.Encoding.UTF8.GetString(Take(stringLength).ToArray());
        }

        /// <summary>
        /// Will read and return the string represented by the following 4-n bytes. In the CAT file, a string is stored as an int (the length of the string), 
        /// and then the string itself. This function will read the int first, and then read the following bytes representing the string, without removing anything.
        /// </summary>
        /// <returns>The string represented by the first 4-n bytes in the stream.</returns>
        public string PeekDynamicString()
        {
            var stringLength = PeekInt32();
            if (ByteCount < stringLength) throw new InvalidDataException("Not enough bytes left to peek at this string. Please let erlend.dahl@sintef.no know.");
            return System.Text.Encoding.UTF8.GetString(Peek(4 + stringLength).Skip(4).ToArray());
        }

        /// <summary>
        /// Will read and return the string represented by the next n bytes, until a null byte is found.
        /// </summary>
        /// <returns>The string represented by the first n-\0 bytes in the stream.</returns>
        public string TakeNullTerminatedString()
        {
            var stringLength = PeekInt32();
            if (ByteCount < stringLength) throw new InvalidDataException("Not enough bytes left to take this string. Please let erlend.dahl@sintef.no know.");
            _position += 4;
            return System.Text.Encoding.UTF8.GetString(Take(stringLength).ToArray());
        }

        /// <summary>
        /// Will print the next n bytes to the Console.
        /// </summary>
        /// <param name="n">The number of bytes to print.</param>
        public void PrintNextBytes(int n)
        {
            for (var i = 0; i < n; i++)
            {
                Console.WriteLine(_bytes[_position + i] + ", " + (char)_bytes[_position + i]);
            }
        }

        /// <summary>
        /// Will verify that the next n bytes are identical to the given bytes, without removing them. If they are not, this function will throw an exception.
        /// </summary>
        /// <param name="bs">The bytes to verify against.</param>
        public bool PeekVerify(byte[] bs)
        {
            if (bs.Length > ByteCount)
                return false;
            for (var i = 0; i < bs.Length; i++)
                if (_bytes[_position + i] != bs[i]) return false;
            return true;
        }

        /// <summary>
        /// Will return a bool value represented by the first byte in the stream.
        /// </summary>
        /// <returns>True if the byte equals 1, otherwise False.</returns>
        public bool TakeBool()
        {
            var b = TakeOne();
            if (b > 1)
                throw new InvalidDataException("Sorry, but this is no bool. Please let erlend.dahl@sintef.no know.");
            return b == 1;
        }

        /// <summary>
        /// Will remove bytes until the target sequence is found.
        /// </summary>
        /// <returns>The removed bytes.</returns>
        /// <param name="find">The sequence to look for.</param>
        /// <param name="removeTargetToo">If the target sequence itself should be removed.</param>
        public byte[] SkipUntil(byte[] find, bool removeTargetToo)
        {
            for (var i = 0; i < ByteCount - find.Length; i++)
            {
                var foundMatch = true;
                for (var a = 0; a < find.Length; a++)
                {
                    if (_bytes[_position + i + a] != find[a])
                    {
                        foundMatch = false;
                        break;
                    }
                }
                if (foundMatch)
                {
                    return Take(i + (removeTargetToo ? find.Length : 0));
                }
            }

            throw new InvalidDataException("The target string could not be found. Please let erlend.dahl@sintef.no know.");
        }

        /// <summary>
        /// Will return a bool value represented by the first two bytes in the stream.
        /// </summary>
        /// <returns>True if the bytes equal [255, 255], otherwise False.</returns>
        public bool TakeLongBool()
        {
            var take = Take(2).ToArray();
            if (take.SequenceEqual(new byte[] { 255, 255 }))
                return true;
            if (take.SequenceEqual(new byte[] { 0, 0 }))
                return false;
            throw new InvalidDataException("This is not a bool! Please let erlend.dahl@sintef.no know.");
        }
    }
}
