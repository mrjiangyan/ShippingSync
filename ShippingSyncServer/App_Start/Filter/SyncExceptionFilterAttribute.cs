
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Utilities.Entity;
using Utilities.Utils;

namespace ShippingSyncServer.Filters
{
    public class SyncExceptionFilterAttribute : ExceptionFilterAttribute
    {
 
        public static void SaveException(HttpActionExecutedContext context)
        {
            var log = new ExceptionLog
            {
                ExceptionName = context.Exception.GetType().ToString(),
                Content = context.Exception.ToString()
            };
            if (context.Exception is BusinessException)
            {
                log.IsBusinessException = true;              
                log.Level = 1;
            }
            else
            {
                log.Level = 2;
            }
            if (context.Request.Content.Headers.ContentLength > 0)
            {
                var request = context.Request;
                var result = context.Request.Content.ReadAsStreamAsync().Result;
                var content = new byte[result.Length];
                result.Position = 0;
                result.Read(content, 0, content.Length);
                var contentStr = Encoding.UTF8.GetString(content);
                log.RequestData = contentStr;
            }
            log.Header = context.Request.Headers.ToString();
            log.HttpMethod = context.Request.Method.ToString();
            log.RequestUrl = context.Request.RequestUri.ToString();
            //记录请求的数据
            if (context.ActionContext.ControllerContext.Controller is PmsApiController)
            {
                var controller = context.ActionContext.ControllerContext.Controller as PmsApiController;
                //记录请求的数据
                if (log.RequestData != null && controller.RequestEntity != null)
                    log.RequestData = controller.RequestEntity.RequestData;
                if (controller.CurrentContextInfo != null)
                {
                    log.OrgId = controller.CurrentContextInfo.CurrentOrgId;
                    if (controller.CurrentContextInfo.CurrentUser != null)
                    {
                        log.OwnerId = controller.CurrentContextInfo.CurrentUser.OwnerId;
                        log.UserName = controller.CurrentContextInfo.CurrentUser.Name;
                    }
                }
            }
            //记录下应用的错误信息
            LogUtility.Error(log);
        }

        public override Task OnExceptionAsync(HttpActionExecutedContext context,
            CancellationToken cancellationToken)
        {
            var result = new ApiResponse<string>();
            var exception = context.Exception as BusinessException;
            Task.Run(() => SaveException(context), cancellationToken);
            Exception innerException = context.Exception;
            while (innerException.InnerException != null)
            {
                innerException = innerException.InnerException;
            }
            result.WithError(innerException.Message);           
            if (context.Response == null)
                context.Response = HttpResponseHelper.GetResponse(result);          
            return base.OnExceptionAsync(context, cancellationToken);
        }
    }
}