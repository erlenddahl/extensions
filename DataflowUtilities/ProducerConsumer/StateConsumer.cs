using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataflowUtilities.ProducerConsumer
{
    public class StateConsumer<TItem, TState> : ConsumerBase
    {
        public TState State { get; }

        public StateConsumer(TState state)
        {
            State = state;
        }

        public async Task<ConsumerBase> Run(BufferBlock<TItem> buffer, Action<TItem, TState> action, Action<TItem, Exception> onException)
        {
            while (await buffer.OutputAvailableAsync())
            {
                while (buffer.TryReceive(out var item))
                {
                    Received++;
                    Process(item, State, action, onException);
                }
            }

            return this;
        }
    }
}
