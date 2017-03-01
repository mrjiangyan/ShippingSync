using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using iPms.WebEntity;
using iPms.WebEntity.Common;
using iPms.WebUtilities.Controller;
using iPms.WebUtilities.Helper;
using iPms.WebUtilities.Models;
using NextPms.Dal.Util;
using NextPms.Logic.BusinessException;
using NextPms.Logic.ServiceDependency;
using NextPms.Util.Time;
using WebApi.Entity;
using NextPms.Util;
using NextPms.Logic.Transaction;

namespace iPms.WebUtilities.Attribute
{
    public class PmsAuthorizeAttribute : AuthorizeAttribute
    {

        public static string userToken = null;

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var header = actionContext.Request.Headers;
            var controller = (BaseController)actionContext.ControllerContext.Controller;
            var info = controller.CurrentContextInfo;
         
            if (info != null && info.CurrentUser != null)
            {
                return true;
            }
            var request = HttpContext.Current.Request;
            var accessKeyId = request.Headers[RequestEntity.RequestHeaders.AccessKeyId];
            var invoker = InvokerUtils.GetInvoker(accessKeyId);

            var apiHeader = request.Headers[RequestEntity.RequestHeaders.ApiHeader];
            controller.Header = apiHeader.JsonObjectDeserialize<ApiHeader>();
            InvokeContext.Invoker = invoker;
            #region 当前会话中不存在用户信息的情况
            if (invoker.EnableSession)
            {
                //从header中获取到用户Token
                 userToken = GetCookie(actionContext.Request.Headers, "SessionId");
                if (userToken == null)
                    throw new BusinessException(string.Empty, (int)HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized);
                var tuple = AuthorizeHelper.DecodezJwtToken(userToken);
                var user = tuple.Item1;
                if (user == null)
                {
                    throw new BusinessException(string.Empty, (int)HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized);
                }
                System.Web.HttpCookie cookie = new HttpCookie("SessionId", AuthorizeHelper.GenerateJwtToken(user));
                cookie.Expires = CurrentTime.Now.AddDays(7);
                HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            return true;
            #endregion
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Content = HttpResponseHelper.GetStandardContent(new ApiResponse<string>().WithError(HttpStatusCode.Unauthorized));

        }

        public static string GetCookie(HttpRequestHeaders httpRequestHeaders, string name)
        {
            try
            {
                var cookies = GetCookies(httpRequestHeaders);
                return cookies.Select(cookie => cookie[name].Value).FirstOrDefault(value => value != null);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static IEnumerable<CookieHeaderValue> GetCookies(HttpRequestHeaders httpRequestHeaders)
        {
            var result = new Collection<CookieHeaderValue>();
            IEnumerable<string> cookies;

            if (!httpRequestHeaders.TryGetValues("Cookie", out cookies)) return result;
            foreach (var cookie in cookies)
            {
                CookieHeaderValue cookieHeaderValue;
                if (CookieHeaderValue.TryParse(cookie, out cookieHeaderValue))
                {
                    result.Add(cookieHeaderValue);
                }
            }
            return result;
        }
    }
}
