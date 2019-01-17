using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpgsqlExtensions.Helpers
{
    public class InsertionBuffer<T> : IDisposable
    {
        private readonly int _insertAt;
        private readonly Action<List<T>> _insertAction;
        public List<T> Items { get; }

        public InsertionBuffer(Action<List<T>> insertAction, int insertAt = 10000)
        {
            _insertAt = insertAt;
            _insertAction = insertAction;
            Items = new List<T>(_insertAt);
        }

        public void Add(T item)
        {
            lock (this)
            {
                Items.Add(item);

                if (Items.Count >= _insertAt)
                {
                    _insertAction(Items);
                    Items.Clear();
                }
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public void Finish()
        {
            lock (this)
            {
                if (Items.Any())
                {
                    _insertAction(Items);
                    Items.Clear();
                }
            }
        }

        public void Dispose()
        {
            Finish();
        }
    }
}
