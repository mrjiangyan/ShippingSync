using iPms.WebEntity;
using iPms.WebUtilities.Models;
using Jose;
using NextPms.Dal.Entity.Statistics;
using NextPms.Logic.BusinessException;
using NextPms.Util.CrypHelper;
using NextPms.Util.Time;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using static NextPms.Dal.Entity.Common.BaseRequestEntity;

namespace iPms.WebUtilities.Helper
{
    public class AuthorizeHelper
    {

        private static readonly byte[] SecretKey =
      {
            164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159,
            209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234
        };

        /// <summary>
        ///     验证请求是否合法
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="invoker"></param>
        /// <returns></returns>
        public static void VerifyV1(RequestEntity entity, AppInvoker invoker)
        {
            
            if (string.IsNullOrEmpty(entity.AccessKeyId))
            {
                throw new BusinessException("AccessKeyId参数丢失", (int)HttpStatusCode.ExpectationFailed,HttpStatusCode.ExpectationFailed);
            }
            DateTime timestamp;
            if (
                !DateTime.TryParseExact(entity.TimeStamp, "yyyy-MM-ddTHH:mm:sszzz", null, DateTimeStyles.AdjustToUniversal,
                    out timestamp))
                throw new ApiException(HttpStatusCode.Forbidden);
            //根据AccessKeyId查找相应的Secret Access Key,a

            //遍历所有自定义的请求头,按照字母的顺序从小到大，以验证完整性
            var sb = new StringBuilder();

            var authStringPrefix = string.Format("{0}/{1}/{2}/{3}", entity.ProtocolVersion, entity.AccessKeyId,
                entity.TimeStamp, entity.ExpirationPeriodInSeconds);

          
            foreach (var headerKey in HttpContext.Current.Request.Headers.AllKeys.OrderBy(x => x))
            {
                var lowerKey = headerKey.ToLower();
                if (RequestHeaders.AccessKeyId.ToLower() == lowerKey
                    || RequestHeaders.ExpirationPeriodInSeconds.ToLower() == lowerKey
                    || RequestHeaders.Language.ToLower() == lowerKey
                    || RequestHeaders.Version.ToLower() == lowerKey
                    || RequestHeaders.Timestamp.ToLower() == lowerKey
                    || RequestHeaders.ApiHeader.ToLower() == lowerKey
                    || RequestHeaders.UserToken.ToLower() == lowerKey
                    )
                    sb.AppendFormat("{0};", lowerKey);
            }
            var header = sb.ToString();
            //遍历查询字符串
            sb = new StringBuilder();
            if (entity.QueryString != null)
            {
                var list = entity.QueryString.AllKeys.OrderBy(x => x);
                foreach (var key in list)
                {
                    sb.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key), entity.QueryString.Get(key));
                }
            }

            //传输数据的签名，该数据的密钥也是经过了SecureKey进行了签名以后的数据

            var content = new byte[entity.Content.Headers.ContentLength.GetValueOrDefault()];
            if (entity.Content != null && entity.Content.Headers.ContentLength != null &&
                entity.Content.Headers.ContentLength.Value > 0)
            {
                entity.Content.ReadAsStreamAsync().Result.Position = 0;
                (entity.Content.ReadAsStreamAsync().Result).Read(content, 0, content.Length);
            }
            var bytearraykey = Encoding.UTF8.GetBytes(invoker.SecureKey);
            var dataSign = Convert.ToBase64String(SignatureHelper.ComputeHMAC(bytearraykey, content));
            var canonicalRequest = string.Format("{0}\n{1}\n{2}\n{3}\n{4}", entity.HttpMethod.ToUpper(), entity.Uri, sb,
                header, dataSign);
            var signingKey = SignatureHelper.ComputeHMAC(invoker.SecureKey, authStringPrefix);
            VerifySignature(signingKey, canonicalRequest, entity.Authorization);
           
        }


        private static void VerifySignature(byte[] signingKey, string canonicalRequest,string authorization)
        {
            var signature =
               Convert.ToBase64String(SignatureHelper.ComputeHMAC(signingKey, Encoding.UTF8.GetBytes(canonicalRequest)));
            if (!signature.Equals(authorization))
                throw new ApiException(HttpStatusCode.Forbidden);
        }

           /// <summary>
        /// 验证请求是否合法
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="invoker"></param>
        /// <returns></returns>
        public static void VerifyEB(RequestEntity entity, AppInvoker invoker)
        {
            //根据AccessKeyId查找相应的Secret Access Key,
            //如果存在端口转换，则需要对地址进行转换
            var originalPort=entity.Request.Headers["X-Forwarded-Port"];
            var uri= entity.Request.Url;
            var url = uri.ToString();
            if (!string.IsNullOrEmpty(originalPort) && !uri.Port.ToString().Equals(originalPort))
            {                
                var port  = entity.Request.Url.Port;
                url = string.Format("{0}://{1}:{2}{3}", uri.Scheme,uri.Host,originalPort,uri.PathAndQuery);                
            }
            var authStringPrefix = string.Format("{0}/{1}/{2}", entity.ProtocolVersion, entity.AccessKeyId,
                entity.TimeStamp);
            var signingKey = SignatureHelper.ComputeHMAC(invoker.SecureKey, authStringPrefix);
            StringBuilder sb = new StringBuilder();
            foreach (var headerKey in entity.Request.Headers.AllKeys.OrderBy(x => x))
            {
                var lowerKey = headerKey.ToLower();
                if (RequestEntity.RequestHeaders.OrgId.ToLower() == lowerKey
                    || RequestEntity.RequestHeaders.OwnerId.ToLower() == lowerKey
                    || RequestEntity.RequestHeaders.Language.ToLower() == lowerKey
                    || RequestEntity.RequestHeaders.Version.ToLower() == lowerKey
                    || RequestEntity.RequestHeaders.Timestamp.ToLower() == lowerKey
                    )
                {
                    sb.Append(lowerKey);
                    sb.Append(entity.Request.Headers[lowerKey]);
                }
                    
            }
            var header = sb.ToString();
            //传输数据的签名，该数据的密钥也是经过了SecureKey进行了签名以后的数据
            var canonicalRequest = string.Format("{0}\n{1}\n{2}", entity.HttpMethod.ToUpper(), url, header);
            VerifySignature(signingKey, canonicalRequest, entity.Authorization);
           
        }

        /// <summary>
        /// 验证外部请求是否合法
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="invoker"></param>
        /// <returns></returns>
        public static void VerifyExternalApi(RequestEntity entity, AppInvoker invoker)
        {
            //如果存在端口转换，则需要对地址进行转换
            var originalPort = entity.Request.Headers["X-Forwarded-Port"];
            var uri = entity.Request.Url;
            var url = uri.ToString();
            if (!string.IsNullOrEmpty(originalPort) && !uri.Port.ToString().Equals(originalPort))
            {

                var port = entity.Request.Url.Port;
                url = string.Format("{0}://{1}:{2}{3}", uri.Scheme, uri.Host, originalPort, uri.PathAndQuery);
            }
            var authStringPrefix = string.Format("{0}/{1}/{2}", entity.ProtocolVersion, entity.AccessKeyId,
                entity.TimeStamp);

            var signingKey = SignatureHelper.ComputeHMAC(invoker.SecureKey, authStringPrefix);
            //传输数据的签名，该数据的密钥也是经过了SecureKey进行了签名以后的数据
            var canonicalRequest = string.Format("{0}\n{1}", entity.HttpMethod.ToUpper(), url);
            VerifySignature(signingKey, canonicalRequest, entity.Authorization);
           
        }

        /// <summary>
        ///     获取用户Token
        /// </summary>
        /// <returns></returns>
        public static string GenerateJwtToken(StatisticsUser user)
        {
            var payload = new Dictionary<string, object>
            {
                { "exp" ,CurrentTime.Now.AddDays(7).ToString()}
            };
            var json = JsonSerializer.SerializeToString(user);
            return JWT.Encode(json, SecretKey, JwsAlgorithm.HS512, payload);
        }

        public static Tuple<StatisticsUser, object> DecodezJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return new Tuple<StatisticsUser, object>(null, null);
                token = token.Trim('\"');
            //兼容老的Token算法
            try
            {
                var user = JWT.Decode<StatisticsUser>(token, SecretKey);
                IDictionary<string, object> dicHeaders= JWT.Headers(token);
                var exp = dicHeaders.FirstOrDefault(o => o.Key == "exp");
                if (Convert.ToDateTime(exp.Value) > CurrentTime.Now)
                {
                    return new Tuple<StatisticsUser, object>(user, null);
                }
            }
            catch
            {
                // ignored
            }
            return new Tuple<StatisticsUser, object>(null, null);
        }


    }
}