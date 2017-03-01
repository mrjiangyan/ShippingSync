using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace iPms.WebUtilities.Helper
{
    public class JsonNetFormatter : JsonMediaTypeFormatter
    {
       
        public JsonNetFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type,
                                                         Stream stream,
                                                         HttpContent content,
                                                         IFormatterLogger formatterLogger)
        {
            var task = Task<object>.Factory.StartNew(() =>
                {
                    
                    var sr = new StreamReader(stream);
                    return ServiceStack.Text.JsonSerializer.DeserializeFromReader(sr, type);
                });

            return task;
        }

        public override Task WriteToStreamAsync(Type type, object value,
                                                Stream stream,
                                                HttpContent content,
                                                TransportContext transportContext)
        {
            var task = Task.Factory.StartNew(() =>
                {
                    ServiceStack.Text.JsonSerializer.SerializeToStream(value, stream);

                });

            return task;
        }
    }
}