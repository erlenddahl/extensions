using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataflowUtilities.ProducerConsumer
{
    public class ConsumerCollection<TItem> : ConsumerCollectionBase<TItem>
    {
        public Action<TItem> ConsumeAction { get; set; }

        public ConsumerCollection(double multiplier, Action<TItem> consumeAction = null) : base()
        {
            ConsumerCount = (int)(Environment.ProcessorCount * multiplier);
            ConsumeAction = consumeAction;
        }

        public override void Run()
        {
            Consumers = Enumerable.Range(0, ConsumerCount).Select(p => new Consumer<TItem>()).Select(p => ((ConsumerBase)p, p.Run(Buffer, ConsumeAction))).ToList();
        }
    }
}
