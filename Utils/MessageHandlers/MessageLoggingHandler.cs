using System;
using System.Text;
using System.Threading.Tasks;
using NextPms.Log;
using NextPms.Log.Error;
using System.Net.Http.Headers;
using static NextPms.Dal.Entity.Common.BaseRequestEntity;
using NextPms.Logic.Transaction;
using NextPms.Util;
using NextPms.Logic;

namespace iPms.WebUtilities.MessageHandlers
{
    public class MessageLoggingHandler : MessageHandler
    {
        protected override async Task LogMessageAsync(HttpRequestHeaders header, string method, string uri, byte[] requestMessage,
            byte[] responseMessage, DateTime beginTime, DateTime endTime)
        {
#if DEBUG
                if (uri.ToLower().IndexOf("swagger") > -1)
                    return;
#endif 
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
            if (InvokeContext.Invoker != null)
                log.Invoker = InvokeContext.Invoker.JsonObjectSerialize();
            Logger.LogApiCall(log);
        }
    }
}