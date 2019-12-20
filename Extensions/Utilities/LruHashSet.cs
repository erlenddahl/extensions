using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Utilities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A least-recently-used cache stored like a dictionary.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type of the key to the cached item
    /// </typeparam>
    /// <typeparam name="TValue">
    /// The type of the cached item.
    /// </typeparam>
    /// <remarks>
    /// Derived from https://stackoverflow.com/a/3719378/240845
    /// </remarks>
    public class LruHashSet<T>
    {
        private readonly HashSet<T> _set = new HashSet<T>();
        private readonly LinkedList<T> _lruList = new LinkedList<T>();

        private readonly Action<T> _dispose;

        public int Count => _set.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="LruCache{TKey, TValue}"/>
        /// class.
        /// </summary>
        /// <param name="capacity">
        /// Maximum number of elements to cache.
        /// </param>
        /// <param name="dispose">
        /// When elements cycle out of the cache, disposes them. May be null.
        /// </param>
        public LruHashSet(int capacity, Action<T> dispose = null)
        {
            Capacity = capacity;
            this._dispose = dispose;
        }

        /// <summary>
        /// Gets the capacity of the cache.
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="value">
        /// The value of the element to add.
        /// </param>
        public void Add(T value)
        {
            lock (_set)
            {
                if (_set.Count >= Capacity)
                {
                    RemoveFirst();
                }

                var node = new LinkedListNode<T>(value);
                _lruList.AddLast(node);
                _set.Add(value);
            }
        }

        public bool Contains(T value)
        {
            lock (_set)
            {
                return _set.Contains(value);
            }
        }

        private void RemoveFirst()
        {
            // Remove from LRUPriority
            var node = _lruList.First;
            _lruList.RemoveFirst();

            // Remove from cache
            _set.Remove(node.Value);

            // dispose
            _dispose?.Invoke(node.Value);
        }
    }
}
