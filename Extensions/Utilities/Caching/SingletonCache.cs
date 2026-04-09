using System;
using System.Collections.Generic;

namespace net.erlenddahl.Extensions.Utilities.Caching
{
    /// <summary>
    /// Simple dictionary based cache meant for lazily created singleton values.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class SingletonCache<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();
        private readonly object _locker = new object();

        /// <summary>
        /// Returns the cached object corresponding to the given key if it exists, or
        /// creates it using the valueCreationFunc, and then adds it to the cache before
        /// returning it.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueCreationFunc"></param>
        /// <returns></returns>
        public TValue Get(TKey key, Func<TValue> valueCreationFunc)
        {
            lock (_locker)
            {
                if (_cache.TryGetValue(key, out var cached)) return cached;
                cached = valueCreationFunc();
                _cache.Add(key, cached);
                return cached;
            }
        }
    }
}
