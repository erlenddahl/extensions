using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataflowUtilities.ProducerConsumer
{
    public class StateConsumer<TItem, TState> : ConsumerBase
    {
        private TState _state;

        public StateConsumer(TState state)
        {
            _state = state;
        }

        public async Task<ConsumerBase> Run(BufferBlock<TItem> buffer, Action<TItem, TState> action)
        {
            while (await buffer.OutputAvailableAsync())
            {
                while (buffer.TryReceive(out var item))
                {
                    Received++;
                    Process(item, _state, action);
                }
            }

            return this;
        }
    }
}
