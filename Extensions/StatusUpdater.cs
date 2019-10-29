using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Extensions
{
    public static class StatusUpdater
    {

        public static async Task<bool> SendProgress(string name, string message, long value, long max)
        {
            return await SendMessage("progress", name, message, 300, value, max);
        }

        public static async Task<bool> SendProgress(string name, string message, double progress)
        {
            return await SendMessage("progress", name, message, 300, -1, -1, progress);
        }

        public static async Task<bool> SendHeartbeat(string name, string message, int timeout = 300, int value = 1)
        {
            return await SendMessage("heartbeat", name, message, timeout, value);
        }

        public static async Task<bool> SendMessage(string name, string message, int timeout = 300, string status = null)
        {
            return await SendMessage("message", name, message, timeout, 1, status: status);
        }

        public static async Task<bool> SendCalendar(string name, string message)
        {
            return await SendMessage("calendar", name, message, 300, 1);
        }

        private static async Task<bool> SendMessage(string datatype, string name, string message, int timeout, long value = -1, long max = -1, double progress = -1, int sequence = 999, string status = null)
        {
            try
            {
                var url = string.Format("http://erlenddahl.no/etc/status/data.php?{4}={0}&value={1}&timeout={2}&sequence={3}&prog_value={5}&prog_max={6}&prog_percentage={7}", HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(message), timeout, sequence, datatype, value, max, progress);
                if (!string.IsNullOrWhiteSpace(status))
                    url += "&status=" + status;
                await DownloadString(url);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return false;
            }

        }

        private static async Task<string> DownloadString(string url)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(2);
                using (var response = await client.GetAsync(url))
                using (var content = response.Content)
                {
                    return await content.ReadAsStringAsync() ?? "";
                }
            }
        }
    }
}
