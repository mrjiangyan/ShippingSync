﻿

using System;
using System.Net;
using System.Text;
using Utilities;
using Utilities.Utils;

namespace iPms.WebUtilities.Helper
{
    public class AuthorizeHelper
    {
        private const string app_securt = "";


      
        private static void VerifySignature(byte[] signingKey, byte[] requestBody,string authorization)
        {
            var signature =
               Convert.ToBase64String(SignatureHelper.ComputeHMAC(signingKey, requestBody));
            if (!signature.Equals(authorization))
                throw new ApiException(HttpStatusCode.Forbidden);
        }


        /// <summary>
        /// 验证外部请求是否合法
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="invoker"></param>
        /// <returns></returns>
        public static void VerifyExternalApi(RequestEntity entity)
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
            var canonicalRequest = string.Format("{0}\n{1}", entity.Request.HttpMethod.ToUpper(), url);

            var signingKey = SignatureHelper.ComputeHMAC(app_securt, canonicalRequest);
            //Get RequestBody;
            var content = new byte[entity.Request.ContentLength];
            var result = entity.Request.GetBufferedInputStream();
            result.Position = 0;
            result.Read(content, 0, content.Length);
            VerifySignature(signingKey, content, entity.Request.Headers["Authorization"]);
           
        }

     
     

    }
}