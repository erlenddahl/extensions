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
    public abstract class ConsumerCollectionBase<TItem>
    {
        public int ConsumerCount { get; set; }

        /// <summary>
        /// The maximum amount of buffer items per consumer before entering a sleep loop.
        /// </summary>
        public int MaxBufferItemsPerConsumer { get; set; } = 1000;
        
        /// <summary>
        /// The number of milliseconds to wait before checking if the number of buffer items is low enough to keep posting.
        /// </summary>
        public int MaxBufferExceededWaitingTime { get; set; } = 200;
        public double TimeLostToFullBuffer { get; private set; } = 0;

        /// <summary>
        /// The number of items that has been posted to this collection.
        /// </summary>
        public int Posted { get; private set; }

        public BufferBlock<TItem> Buffer { get; private set; }
        public List<(ConsumerBase consumer, Task<ConsumerBase> task)> Consumers { get; protected set; }
        
        protected ConsumerCollectionBase()
        {
            Buffer = new BufferBlock<TItem>();
        }

        public abstract void Run();

        public void Post(TItem item)
        {
            if (Consumers == null) Run();

            var start = DateTime.Now;
            while (Buffer.Count >= MaxBufferItemsPerConsumer * ConsumerCount)
                Thread.Sleep(MaxBufferExceededWaitingTime);
            TimeLostToFullBuffer += DateTime.Now.Subtract(start).TotalSeconds;

            Buffer.Post(item);
            Posted++;
        }

        public void Complete(bool waitForConsumers = true)
        {
            Buffer.Complete();

            if(waitForConsumers)
                Task.WaitAll(Consumers.Select(p => p.task).ToArray<Task>());
        }

        public void Complete(Action performWhileWaiting, int waitInterval = 500)
        {
            Buffer.Complete();
            while (Buffer.Count > 0)
            {
                performWhileWaiting();
                Thread.Sleep(waitInterval);
            }
            Task.WaitAll(Consumers.Select(p => p.task).ToArray<Task>());
        }
    }
}
