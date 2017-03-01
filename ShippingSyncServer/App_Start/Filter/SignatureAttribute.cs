using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ShippingSyncServer.Filters
{
    public class PmsSignatureAttribute : ActionFilterAttribute
    {
     
      
        public async override  Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var apiController = actionContext.ControllerContext.Controller as PmsApiController;
             //对数据进行校验
            var context = HttpContext.Current;
            var header = context.Request.Headers;
            var entity = new RequestEntity
            {
                ProtocolVersion = header[RequestEntity.RequestHeaders.Version],
                AccessKeyId = header[RequestEntity.RequestHeaders.AccessKeyId],
                TimeStamp = header[RequestEntity.RequestHeaders.Timestamp],
                Authorization = header[RequestEntity.RequestHeaders.Authorization],
                OwnerId = header[RequestEntity.RequestHeaders.OwnerId],
                OrgId = header[RequestEntity.RequestHeaders.OrgId],
                Language = header[RequestEntity.RequestHeaders.Language],
                HttpMethod = HttpContext.Current.Request.HttpMethod,
                Uri = context.Request.Url.AbsolutePath,
                Request = context.Request,
                QueryString = context.Request.QueryString,
                Content = actionContext.Request.Content
            };

            if (header.AllKeys.Contains(RequestEntity.RequestHeaders.ExpirationPeriodInSeconds))
                int.TryParse(header[RequestEntity.RequestHeaders.ExpirationPeriodInSeconds],
                    out entity.ExpirationPeriodInSeconds);

            //新增加的RequestId，方便标记每一次的请求与追溯
            entity.RequestId = header[BaseRequestEntity.RequestHeaders.RequestId];
            if ((apiController != null) && (header[BaseRequestEntity.RequestHeaders.ApiHeader] != null))
            {
                apiController.RequestEntity = entity;
                try
                {
                    //从请求内获取到公共的ApiHeader相关的数据
                    apiController.Header =
                        JsonSerializer.DeserializeFromString<ApiHeader>(
                            header[RequestHeaders.ApiHeader]);
                }
                catch
                {
                    // ignored
                }
            }
            var accessKeyId=string.Empty;
            var values = header.GetValues(RequestEntity.RequestHeaders.AccessKeyId);
            if (values != null)
                accessKeyId = values.First();
            var invoker = InvokerUtils.GetInvoker(accessKeyId);
            var controller = actionContext.ControllerContext.Controller as BaseController;
            InvokeContext.Invoker = invoker;

            //如果发生认证失败的情况
            if (!invoker.EnableSession)
            {
                Action<RequestEntity, AppInvoker> action = AuthorizeHelper.VerifyV1;
                if (entity.ProtocolVersion == "EB-auth-v1")
                {
                    action = AuthorizeHelper.VerifyEB;
                }
                //以下代码在正式发布的时候必须要打开，否则会有安全漏洞
#if !DEBUG && !PublicTest
                action.Invoke(entity,invoker);
#endif
            }
            foreach (var parameter in actionContext.ActionDescriptor.ActionBinding.ParameterBindings)
            {   
                if (!parameter.WillReadBody || !(parameter.Descriptor.ParameterBinderAttribute is FromBodyAttribute))
                    continue;
                var content = new byte[entity.Content.Headers.ContentLength.Value];
                var result = await entity.Content.ReadAsStreamAsync();
                result.Position = 0;
                result.Read(content, 0, content.Length);
                var targetType = parameter.Descriptor.ParameterType;
                var contentStr = Encoding.UTF8.GetString(content);
                entity.RequestData = contentStr;
                actionContext.ActionArguments[parameter.Descriptor.ParameterName] =
                    contentStr.JsonObjectDeserialize(targetType);
                //因为只能有一个FromBodyAttribute类型的参数，所以执行以后就可以break；
                break;
            }
            
            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }


        /// <summary>
        ///     输出结果的方法
        /// </summary>
        /// <param name="actContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actContext)
        {
            if ((actContext.Response != null) && (actContext.Response.Headers.CacheControl != null))
            {
                base.OnActionExecuted(actContext);
                return;
            }
            var result = new ApiResponse<string>();
            if(actContext.Exception!= null)
            {
                Task.Run(() => PmsExceptionFilterAttribute.SaveException(actContext));
            }
                
            string message;
            Exception innerException = actContext.Exception;
            if (innerException!= null)
            {
                while (innerException.InnerException != null)
                {
                    innerException = innerException.InnerException;
                }
            }          
            if (innerException is BusinessException)
            {
                var businessException = innerException as BusinessException;
                result.WithError(businessException.Message, businessException.ErrorCode);
            }
            else if (innerException != null)
            {
                message = innerException.Message;
                result.WithError(message);
            }
            if (actContext.Response == null || innerException != null)
            {
                actContext.Response = HttpResponseHelper.GetResponse(result);
            }
            else
            {
                var value = ((ObjectContent)actContext.Response.Content).Value;
                var apiResult = value as BaseResponse;
                if (apiResult != null)
                {
                    actContext.Response.StatusCode = (HttpStatusCode)apiResult.resultCode;
                }
                actContext.Response.Content = HttpResponseHelper.GetStandardContent(value);
            }
            //对结果进行处理
            base.OnActionExecuted(actContext);
        }
    }
}