using System.Collections.Specialized;
using System.Net.Http;
using System.Web;
using NextPms.Dal.Entity.Common;

namespace iPms.WebUtilities.Models
{
    /// <summary>
    /// 发送数据请求时的实体
    /// </summary>
    public class RequestEntity : BaseRequestEntity
    {
        /// <summary>
        /// 传输的原始数据，即可能是被压缩的或者原始数据
        /// </summary>
        public HttpContent Content;

        public HttpRequest Request { get; set; }


    }
}