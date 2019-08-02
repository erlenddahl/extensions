using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataflowUtilities.ProducerConsumer
{
    public class QueuedConsumer<T> : ConsumerBase
    {
        private readonly int _queueSize;

        public QueuedConsumer(int queueSize)
        {
            _queueSize = queueSize;
        }

        public async Task<QueuedConsumer<T>> Run(BufferBlock<T> buffer, Action<T> action)
        {
            var todo = new List<T>();
            while (await buffer.OutputAvailableAsync())
            {
                while (buffer.TryReceive(out var item))
                {
                    todo.Add(item);
                    Received++;

                    if (todo.Count >= _queueSize)
                    {
                        foreach (var t in todo)
                            Process(t, action);
                        todo.Clear();
                    }
                }
            }

            foreach (var t in todo)
                Process(t, action);
            todo.Clear();

            return this;
        }
    }
}
