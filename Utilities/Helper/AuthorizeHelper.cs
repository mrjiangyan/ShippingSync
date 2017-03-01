

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Utilities.Utils;

namespace iPms.WebUtilities.Helper
{
    public class AuthorizeHelper
    {

        private static readonly byte[] SecretKey =
      {
            164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159,
            209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234
        };

      
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

     
     

    }
}