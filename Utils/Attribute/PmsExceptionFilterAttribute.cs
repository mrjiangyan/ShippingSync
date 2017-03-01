using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using iPms.WebEntity.Common;
using iPms.WebUtilities.Controller;
using iPms.WebUtilities.Models;
using NextPms.Logic.BusinessException;
using NextPms.Logic.Transaction;
using NextPms.Util;
using NextPms.Log;
using NextPms.Log.Error;

namespace iPms.WebUtilities.Attribute
{
    public class PmsExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private static readonly string AccessKeyIdType = RequestEntity.RequestHeaders.AccessKeyId.ToString();

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
                var iPmsController = context.ActionContext.ControllerContext.Controller as PmsApiController;
                //记录请求的数据
                if (log.RequestData != null &&iPmsController.RequestEntity != null)
                    log.RequestData = iPmsController.RequestEntity.RequestData;
                if (iPmsController.CurrentContextInfo != null)
                {
                    log.OrgId = iPmsController.CurrentContextInfo.CurrentOrgId;
                    if (iPmsController.CurrentContextInfo.CurrentUser != null)
                    {
                        log.OwnerId = iPmsController.CurrentContextInfo.CurrentUser.OwnerId;
                        log.UserName = iPmsController.CurrentContextInfo.CurrentUser.Name;
                    }
                }
            }
            if(InvokeContext.Invoker!= null)
                log.Invoker= InvokeContext.Invoker.JsonObjectSerialize();
            //记录下应用的错误信息
            Logger.LogException(log);
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