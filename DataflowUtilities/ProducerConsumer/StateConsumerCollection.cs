using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataflowUtilities.ProducerConsumer
{
    public class StateConsumerCollection<TItem, TState> : ConsumerCollectionBase<TItem>
    {
        public TState[] States { get; private set; }
        public Action<TItem, TState> ConsumeAction { get; set; }

        public Func<TState> StateGenerator { get; set; }

        /// <summary>
        /// Contains all exceptions from all consumers, if <see cref="TrackExceptions"/>> is true. Otherwise null.
        /// </summary>
        public List<Exception>? AllExceptions { get; set; }

        /// <summary>
        /// If set to true, all exceptions from all consumers will be stored in <see cref="AllExceptions"/>.
        /// </summary>
        public bool TrackExceptions { get; set; }

        public StateConsumerCollection(IEnumerable<TState> states) : base()
        {
            States = states.ToArray();
            ConsumerCount = States.Length;
        }

        public StateConsumerCollection() : base()
        {
        }

        public override void Run()
        {
            if (States == null || !States.Any())
                States = Enumerable.Range(0, ConsumerCount).Select(p => StateGenerator()).AsParallel().ToArray();

            if (TrackExceptions)
            {
                AllExceptions = new List<Exception>();
            }

            Consumers = States
                .Select(p => new StateConsumer<TItem, TState>(p))
                .Select(p => ((ConsumerBase)p, p.Run(Buffer, ConsumeAction, (item, ex) =>
                {
                    if (TrackExceptions)
                    {
                        AllExceptions.Add(ex);
                    }
                    OnException(item, ex);
                }))).ToList();
        }
    }
}
