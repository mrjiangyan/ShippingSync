using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Utilities.Utils;

namespace ShippingSyncServer
{
    public class MessageLoggingHandler : MessageHandler
    {
        protected override async Task LogMessageAsync(HttpRequestHeaders header, string method, string uri, byte[] requestMessage,
            byte[] responseMessage, DateTime beginTime, DateTime endTime)
        {
                await Task.Run(() =>
                Log(header, method, uri, Encoding.UTF8.GetString(requestMessage), Encoding.UTF8.GetString(responseMessage), beginTime, endTime));
        }

        private static void Log(HttpRequestHeaders header, string method, string uri, string requestContent, string responseContent, DateTime beginTime, DateTime endTime)
        {
            var log = new
                {
                    Method = method,
                    Header= header.ToString(),
                    Request = requestContent,
                    Response = responseContent,
                    Uri = uri,
                    BeginTime = beginTime,
                    EndTime = endTime
                };


            LogUtility.Info(log);
        }
    }
}