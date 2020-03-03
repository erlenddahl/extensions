using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataflowUtilities.ProducerConsumer
{
    public abstract class ConsumerBase
    {
        private static int _idSequence = 0;
        public int ConsumerId { get; } = _idSequence++;

        public int Failed { get; protected set; } = 0;
        public int Received { get; protected set; } = 0;
        public int Processed { get; protected set; } = 0;
        public DateTime LastAction { get; protected set; } = DateTime.Now;
        public (DateTime time, Exception ex) LastException { get; protected set; }

        protected void Process<T>(T t, Action<T> action, Action<T, Exception> onException)
        {
            try
            {
                action(t);
            }
            catch (Exception ex)
            {
                Failed++;
                LastException = (DateTime.Now, ex);

                onException?.Invoke(t, ex);

                if (ex.InnerException != null)
                {
                    Debug.WriteLine("-----");
                    Debug.WriteLine("INNER");
                    Debug.WriteLine("-----");
                    Debug.WriteLine(ex.InnerException.Message);
                    Debug.WriteLine(ex.InnerException.StackTrace);
                    Debug.WriteLine("-----");
                    Debug.WriteLine("OUTER");
                    Debug.WriteLine("-----");
                }

                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            Processed++;
            LastAction = DateTime.Now;
        }

        protected void Process<TItem, TState>(TItem t, TState state, Action<TItem, TState> action, Action<TItem, Exception> onException)
        {
            try
            {
                action(t, state);
            }
            catch (Exception ex)
            {
                Failed++;
                LastException = (DateTime.Now, ex);

                onException?.Invoke(t, ex);

                if (ex.InnerException != null)
                {
                    Debug.WriteLine("-----");
                    Debug.WriteLine("INNER");
                    Debug.WriteLine("-----");
                    Debug.WriteLine(ex.InnerException.Message);
                    Debug.WriteLine(ex.InnerException.StackTrace);
                    Debug.WriteLine("-----");
                    Debug.WriteLine("OUTER");
                    Debug.WriteLine("-----");
                }

                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            Processed++;
            LastAction = DateTime.Now;
        }
    }
}
