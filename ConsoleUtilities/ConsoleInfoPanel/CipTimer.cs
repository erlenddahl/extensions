using System.Diagnostics;

namespace net.erlenddahl.ConsoleUtilities.ConsoleInfoPanel
{
    public class CipTimer : IDisposable
    {
        private readonly ConsoleInformationPanel _cip;
        private readonly string _key;
        private readonly Stopwatch _sw;

        public CipTimer(ConsoleInformationPanel cip, string key)
        {
            _cip = cip;
            _key = key;
            _sw = new Stopwatch();
        }

        public void Dispose()
        {
            var elapsed = _sw.ElapsedMilliseconds;
            _cip.Increment(_key, elapsed);
        }
    }
}