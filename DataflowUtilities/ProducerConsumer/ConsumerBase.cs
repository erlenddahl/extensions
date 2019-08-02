using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataflowUtilities.ProducerConsumer
{
    public abstract class ConsumerBase
    {
        private static int _idSequence = 0;
        public int ConsumerId { get; } = _idSequence++;

        public int Received { get; protected set; } = 0;
        public int Processed { get; protected set; } = 0;
        public DateTime LastAction { get; protected set; } = DateTime.Now;
        public (DateTime time, Exception ex) LastException { get; protected set; }

        protected void Process<T>(T t, Action<T> action)
        {
            action(t);
            Processed++;
            LastAction = DateTime.Now;
        }
    }
}
