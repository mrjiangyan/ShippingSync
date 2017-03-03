using iPms.WebUtilities.Helper;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Utilities;

namespace ShippingSyncServer.Filters
{
    /// <summary>
    /// 外部API访问认证属性
    /// </summary>
    public class SyncAuthorizeAttribute : AuthorizeAttribute
    {
        //对数据进行校验
        protected  override bool IsAuthorized(HttpActionContext actionContext)
        {
            //从接口获取到
            //PMS调用接口时需要设置两个Header,Date和Authorization Date格式为“yyyy-MM-dd HH:mm:ss”,与美团服务器时间误差不能超过2小时            
            var context = HttpContext.Current;
            var header = context.Request.Headers;
            //根据几个数值对数据进行校验
            AuthorizeHelper.VerifyExternalApi(new RequestEntity
            {
                Request = context.Request,               
            });
            return true;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            //actionContext.Response.Content = HttpResponseHelper.GetStandardContent(new ApiResponse<string>().WithError(HttpStatusCode.Unauthorized));
        }
    }
}
