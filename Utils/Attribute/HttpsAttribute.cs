using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace iPms.WebUtilities.Attribute
{
    /// <summary>
    /// 打上该标记的接口，必须强制使用Https
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpsAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {

            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

       

    }
}
