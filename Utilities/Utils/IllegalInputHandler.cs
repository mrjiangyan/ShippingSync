using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.Utils
{
    public class IllegalInputHandler : DelegatingHandler
    {
        private static Dictionary<string, string> illegalInputDict = new Dictionary<string, string>();

        public IllegalInputHandler()
            : base()
        {
            illegalInputDict.Add("<", "&lt;");
            illegalInputDict.Add(">", "&gt;");
            illegalInputDict.Add("\\(", "&#40;");
            illegalInputDict.Add("\\)", "&#41;");
            illegalInputDict.Add("'", "&#39;");
            illegalInputDict.Add("eval\\((.*)\\)", "");
            illegalInputDict.Add("[\\\"\\\'][\\s]*javascript:(.*)[\\\"\\\']", "\"\"");
            // illegalInputDict.Add("script", "");  // 暂时注释掉，避免前台包含description字段传回至后台时被替换成 deion 字段
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var oldUrl = request.RequestUri;
            var oldUrlQuery = oldUrl.Query;

            var oldContent = request.Content;
            var oldContentHeaders = oldContent.Headers;
            var oldContentBody = await oldContent.ReadAsStringAsync();

            var filteredContentBody = DoFilter(oldContentBody);    // 过滤body
            var newHttpContent = new StringContent(filteredContentBody);
            var newContentHeaders = newHttpContent.Headers;
            newContentHeaders.Clear();
            foreach (var item in oldContentHeaders)
            {
                newContentHeaders.Add(item.Key, item.Value);
            }
            request.Content = newHttpContent;

            var newUrl = request.RequestUri.AbsoluteUri;
            var newUrlQuery = oldUrlQuery;

            if (!string.IsNullOrWhiteSpace(oldUrlQuery))
            {
                newUrlQuery = DoFilter(oldUrlQuery);      // 过滤QueryString
                var baseUrl = request.RequestUri.AbsoluteUri.Replace(oldUrlQuery, string.Empty);   // 将queryString部分替换掉
                newUrl = string.Format("{0}&{1}", baseUrl, newUrlQuery);
            }

            request.RequestUri = new Uri(newUrl);

            return await base.SendAsync(request, cancellationToken);
        }

        private static string DoFilter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            foreach (var entry in illegalInputDict)
            {
                input = input.Replace(entry.Key, entry.Value);
            }

            return input;
        }
    }
}
