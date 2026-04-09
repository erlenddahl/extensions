using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace net.erlenddahl.Extensions.Utilities
{
    public class SequenceGenerator : IEnumerable<int>, IEnumerator<int>
    {
        private readonly List<int> _items = new List<int>();

        private SequenceGenerator(int from, int count, int step)
        {
            _items.AddRange(Generate(from, count, step));
        }

        private IEnumerable<int> Generate(int start, int count, int step)
        {
            var value = start;
            for (var i = 0; i < count; i++)
            {
                yield return value;
                value += step;
            }
        }

        public static SequenceGenerator Start(int from, int count, int step = 1)
        {
            return new SequenceGenerator(from, count, step);
        }

        public SequenceGenerator Then(int count, int step = 1)
        {
            _items.AddRange(Generate(_items.Last() + step, count, step));
            return this;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int _position = -1;
        public bool MoveNext()
        {
            _position++;
            return (_position < _items.Count);
        }

        public void Reset()
        {
            _position = -1;
        }

        public int Current => _items[_position];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _items.Clear();
        }
    }
}
