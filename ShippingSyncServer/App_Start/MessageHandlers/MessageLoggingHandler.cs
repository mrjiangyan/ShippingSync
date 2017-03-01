using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;


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
            var log = new RemoteMessageLog
                {
                    Method = method,
                    Header= header.ToString(),
                    Request = requestContent,
                    Response = responseContent,
                    Uri = uri,
                    BeginTime = beginTime,
                    EndTime = endTime
                };
            if(header.Contains(RequestHeaders.OwnerId))
            {
                long ownerId = -1;
                string[] values = (string[])header.GetValues(RequestHeaders.OwnerId);
                if (long.TryParse(values[0], out ownerId))
                    log.OwnerId = ownerId;
            }
            if (header.Contains(RequestHeaders.OrgId))
            {
                long orgId = -1;
                string[] values = (string[])header.GetValues(RequestHeaders.OrgId);
                if (long.TryParse(values[0], out orgId))
                    log.OrgId = orgId;
            }
            Logger.LogApiCall(log);
        }
    }
}