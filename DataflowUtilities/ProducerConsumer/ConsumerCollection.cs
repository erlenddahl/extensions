namespace Sintef.DataflowUtilities.ProducerConsumer
{
    public class ConsumerCollection<TItem> : ConsumerCollectionBase<TItem>
    {
        public Action<TItem> ConsumeAction { get; set; }

        public ConsumerCollection(double multiplier, Action<TItem> consumeAction = null) : base()
        {
            ConsumerCount = (int)(Environment.ProcessorCount * multiplier);
            ConsumeAction = consumeAction;
        }

        public ConsumerCollection() : base()
        {
        }

        public override void Run()
        {
            Consumers = Enumerable.Range(0, ConsumerCount).Select(p => new Consumer<TItem>()).Select(p => ((ConsumerBase)p, p.Run(Buffer, ConsumeAction, OnException))).ToList();
        }
    }
}
