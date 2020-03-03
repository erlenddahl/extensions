using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataflowUtilities.ProducerConsumer
{
    public class Consumer<T> : ConsumerBase
    {
        public async Task<ConsumerBase> Run(BufferBlock<T> buffer, Action<T> action, Action<T, Exception> onException)
        {
            while (await buffer.OutputAvailableAsync())
            {
                while (buffer.TryReceive(out var item))
                {
                    Received++;
                    Process(item, action, onException);
                }
            }

            return this;
        }
    }
}
