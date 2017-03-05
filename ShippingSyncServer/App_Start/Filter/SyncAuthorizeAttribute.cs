
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Utilities;
using Utilities.Entity;
using Utilities.Helper;

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
            //根据几个数值对数据进行校验
            AuthorizeHelper.VerifySyncApi(new RequestEntity
            {
                Request = HttpContext.Current.Request,               
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
