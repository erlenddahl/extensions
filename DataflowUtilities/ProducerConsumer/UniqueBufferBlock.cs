using System.Threading.Tasks.Dataflow;

namespace Sintef.DataflowUtilities.ProducerConsumer
{
    public class BufferBlockDistinctWrapper<T>
    {
        private readonly BufferBlock<T> _buffer;
        private readonly HashSet<T> _hashSet;
        private readonly object _locker = new object();

        public BufferBlockDistinctWrapper(BufferBlock<T> buffer, IEqualityComparer<T> equalityComparer = null)
        {
            _buffer = buffer;
            _hashSet = new HashSet<T>(equalityComparer);
        }

        public void Post(T value)
        {
            lock (_locker)
                if (_hashSet.Add(value))
                    _buffer.Post(value);
        }
    }
}
