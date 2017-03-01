using System.Net;
using System.Net.Http;
using iPms.WebEntity;
using iPms.WebEntity.Common;
using iPms.WebUtilities.Helper;

namespace iPms.WebUtilities.Attribute
{
    public static class HttpResponseHelper
    {
        public static HttpResponseMessage GetResponse(ApiResponse<string> result)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = GetStandardContent(result)
            };
            return response;
        }

        public static ObjectContent<T> GetStandardContent<T>(T result)
        {
            return new ObjectContent<T>(
                result,
                new JsonNetFormatter(),
                "application/json"
                );
        }
    }
}
