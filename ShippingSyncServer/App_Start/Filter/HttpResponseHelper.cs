using System.Net;
using System.Net.Http;


namespace ShippingSyncServer.Filters
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
