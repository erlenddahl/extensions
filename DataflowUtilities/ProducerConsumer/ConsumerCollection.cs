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
    public class ConsumerCollection<T>
    {
        public int ConsumerCount { get; set; }
        public int MaxBufferItemsPerConsumer { get; set; } = 1000;
        public int MaxBufferExceededWaitingTime { get; set; } = 200;
        public double TimeLostToFullBuffer { get; set; } = 0;
        public Action<T> ConsumeAction { get; set; }

        public BufferBlock<T> Buffer { get; private set; }
        public List<(ConsumerBase consumer, Task<ConsumerBase> task)> Consumers { get; private set; }

        public ConsumerCollection(double multiplier, Action<T> consumeAction = null)
        {
            ConsumerCount = (int)(Environment.ProcessorCount * multiplier);
            Buffer = new BufferBlock<T>();
            ConsumeAction = consumeAction;
        }

        public void Run()
        {
            Consumers = Enumerable.Range(0, ConsumerCount).Select(p => new Consumer<T>()).Select(p => ((ConsumerBase)p, p.Run(Buffer, ConsumeAction))).ToList();
        }

        public void Post(T item)
        {
            if (Consumers == null) Run();

            var start = DateTime.Now;
            while (Buffer.Count >= MaxBufferItemsPerConsumer * ConsumerCount)
                Thread.Sleep(MaxBufferExceededWaitingTime);
            TimeLostToFullBuffer += DateTime.Now.Subtract(start).TotalSeconds;

            Buffer.Post(item);
        }

        public void Complete(bool waitForConsumers = true)
        {
            Buffer.Complete();

            if(waitForConsumers)
                Task.WaitAll(Consumers.Select(p => p.task).ToArray<Task>());
        }
    }
}
