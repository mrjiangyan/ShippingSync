using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;


namespace ShippingSyncServer.Filters
{
    /// <summary>
    /// 外部API访问认证属性
    /// </summary>
    public class ExternalAuthorizeAttribute : AuthorizeAttribute
    {
        //对数据进行校验
        protected  override bool IsAuthorized(HttpActionContext actionContext)
        {
            //从接口获取到
            //PMS调用接口时需要设置两个Header,Date和Authorization Date格式为“yyyy-MM-dd HH:mm:ss”,与美团服务器时间误差不能超过2小时            
            var context = HttpContext.Current;
            var header = context.Request.Headers;
            //以下的条件只对测试和公测生效，对于生产不生效
#if DEBUG || PublicTest
            if(string.IsNullOrEmpty(header[RequestHeaders.AccessKeyId]))
                header[RequestHeaders.AccessKeyId] = "f32524fb-44d4-11e6-9b87-000c2948eb8f";

#endif
 
            //根据几个数值对数据进行校验
            var invoker = InvokerUtils.GetInvoker(header[RequestHeaders.AccessKeyId]);
            AuthorizeHelper.VerifyExternalApi(new RequestEntity()
            {
                ProtocolVersion = header[RequestHeaders.Version],
                AccessKeyId = header[RequestHeaders.AccessKeyId],
                TimeStamp = header[RequestHeaders.Timestamp],
                Authorization = header[RequestHeaders.Authorization],
                Language = header[RequestEntity.RequestHeaders.Language],
                HttpMethod = HttpContext.Current.Request.HttpMethod,
                Uri = context.Request.Url.AbsolutePath,
                Request = context.Request,
                QueryString = context.Request.QueryString,
                Content = actionContext.Request.Content,
            }, invoker);
            InvokeContext.Invoker = invoker;
            return true;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Content = HttpResponseHelper.GetStandardContent(new ApiResponse<string>().WithError(HttpStatusCode.Unauthorized));
        }
    }
}
