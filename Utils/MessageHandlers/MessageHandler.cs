using System;
using NextPms.Util.Time;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace iPms.WebUtilities.MessageHandlers
{
    public abstract class MessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var beginTime = CurrentTime.Now;
            var requestMessage = await request.Content.ReadAsByteArrayAsync();
            var response = await base.SendAsync(request, cancellationToken);
            byte[] responseMessage;

            if (response.IsSuccessStatusCode)
                responseMessage = await response.Content.ReadAsByteArrayAsync();
            else
                responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);

            var endTime = CurrentTime.Now;
            await
                LogMessageAsync(request.Headers, request.Method.ToString(), request.RequestUri.ToString(), requestMessage,
                    responseMessage, beginTime, endTime);

            return response;
        }

        protected abstract Task LogMessageAsync(HttpRequestHeaders httpHead, string method, string uri, byte[] requestMessage, byte[] responseMessage, DateTime beginTime, DateTime endTime);
    }
}