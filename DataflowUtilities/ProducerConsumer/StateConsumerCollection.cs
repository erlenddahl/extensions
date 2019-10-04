using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataflowUtilities.ProducerConsumer
{
    public class StateConsumerCollection<TItem, TState> : ConsumerCollectionBase<TItem>
    {
        private TState[] _states;
        public Action<TItem, TState> ConsumeAction { get; set; }

        public StateConsumerCollection(IEnumerable<TState> states, Action<TItem> consumeAction = null) : base()
        {
            _states = states.ToArray();
            ConsumerCount = _states.Length;
        }

        public StateConsumerCollection(double multiplier, Func<TState> stateGenerator, Action<TItem> consumeAction = null) : base()
        {
            ConsumerCount = (int)(Environment.ProcessorCount * multiplier);
            _states = Enumerable.Range(0, ConsumerCount).Select(p => stateGenerator()).ToArray();
        }

        public override void Run()
        {
            Consumers = _states.Select(p => new StateConsumer<TItem, TState>(p)).Select(p => ((ConsumerBase)p, p.Run(Buffer, ConsumeAction))).ToList();
        }
    }
}
