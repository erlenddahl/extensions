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
        /// The average number of seconds since each consumer processed an item.
        /// </summary>
        public double AverageProcessAge => Consumers?.Any() == true ? Consumers.Average(p => DateTime.Now.Subtract(p.consumer.LastAction).TotalSeconds) : 0;

        /// <summary>
        /// The maximum number of seconds since a consumer processed an item.
        /// </summary>
        public double MaxProcessAge => Consumers?.Any() == true ? Consumers.Max(p => DateTime.Now.Subtract(p.consumer.LastAction).TotalSeconds) : 0;

        /// <summary>
        /// The most recent exceptions of all consumers that had any exceptions.
        /// </summary>
        public IEnumerable<(DateTime, Exception)> LastExceptions => Consumers?.Select(p => p.consumer.LastException).Where(p => p.ex != null) ?? new (DateTime, Exception)[0];

        /// <summary>
        /// Will be called when an item causes an exception.
        /// </summary>
        public Action<TItem, Exception> OnException;

        /// <summary>
        /// Returns true if the buffer currently holds more than the allowed number of items per consumer.
        /// </summary>
        public bool IsBufferLimitExceeded => ConsumerCount > 0 && Buffer.Count >= (long)MaxBufferItemsPerConsumer * ConsumerCount;

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

        public void Post(TItem item, bool ignoreBufferLimit = false, Action performWhileWaiting = null)
        {
            if (ConsumerCount < 1) throw new Exception("ConsumerCount cannot be less than 1.");
            if (Consumers == null || !Consumers.Any()) Run();

            if (!ignoreBufferLimit)
            {
                WaitForBufferLimit(performWhileWaiting);
            }

            Buffer.Post(item);
            Posted++;
        }

        /// <summary>
        /// Sleeps the current thread until the number of items in the buffer is below the allowed number
        /// of items per consumer.
        /// </summary>
        public void WaitForBufferLimit(Action performWhileWaiting = null)
        {
            var start = DateTime.Now;
            while (IsBufferLimitExceeded)
            {
                performWhileWaiting?.Invoke();
                Thread.Sleep(MaxBufferExceededWaitingTime);
            }

            TimeLostToFullBuffer += DateTime.Now.Subtract(start).TotalSeconds;
        }

        public void Complete(bool waitForConsumers = true)
        {
            Buffer.Complete();

            if (Consumers?.Any() != true) return;

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

            if (Consumers?.Any() != true) return;

            Task.WaitAll(Consumers.Select(p => p.task).ToArray<Task>());
        }
    }
}
