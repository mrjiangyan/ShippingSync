

using Jose;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Utilities;
using Utilities.Entity;
using Utilities.Utils;

namespace Utilities.Helper
{
    public class AuthorizeHelper
    {
        private const string APP_SECRET = "";
        private static readonly byte[] SecretKey =
       {
            164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159,
            209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234
        };


        private static void VerifySignature(byte[] signingKey, byte[] requestBody,string originSignature)
        {
            var signature =
               Convert.ToBase64String(HMACHelper.ComputeHMAC(signingKey, requestBody));
            if (!signature.Equals(originSignature))
                throw new ApiException(HttpStatusCode.Forbidden);
        }


        /// <summary>
        /// 验证外部请求是否合法
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="invoker"></param>
        /// <returns></returns>
        public static void VerifySyncApi(RequestEntity entity)
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

            var signingKey = HMACHelper.ComputeHMAC(APP_SECRET, canonicalRequest);
            //Get RequestBody;
            var content = new byte[entity.Request.ContentLength];
            var result = entity.Request.GetBufferedInputStream();
            result.Position = 0;
            result.Read(content, 0, content.Length);
            var authorization=entity.Request.Headers["Authorization"];
            string originSignature = authorization.Split(':')[1];
            VerifySignature(signingKey, content, originSignature);
           
        }

        public static string GenerateAuthorization(string app_id,string url,string httpMethod,byte[] requestBody)
        {
            var canonicalRequest = string.Format("{0}\n{1}", httpMethod, url);
            var signingKey = HMACHelper.ComputeHMAC(APP_SECRET, canonicalRequest);
            var signature =
               Convert.ToBase64String(HMACHelper.ComputeHMAC(signingKey, requestBody));
            return "MWS" + " " + app_id + ":" + signature;
        }

        /// <summary>
        ///     获取用户Token
        /// </summary>
        /// <returns></returns>
        public static string GenerateJwtToken(User user)
        {
            var payload = new Dictionary<string, object>
            {
                { "exp" ,DateTime.Now.AddDays(7).ToString()}
            };
            var json = JsonConvert.SerializeObject(user);
            return JWT.Encode(json, SecretKey, JwsAlgorithm.HS512, payload);
        }

        public static Tuple<User, object> DecodezJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return new Tuple<User, object>(null, null);
            token = token.Trim('\"');
            //兼容老的Token算法
            try
            {
                var user = JWT.Decode<User>(token, SecretKey);
                IDictionary<string, object> dicHeaders = JWT.Headers(token);
                var exp = dicHeaders.FirstOrDefault(o => o.Key == "exp");
                if (Convert.ToDateTime(exp.Value) > DateTime.Now)
                {
                    return new Tuple<User, object>(user, null);
                }
            }
            catch
            {
                // ignored
            }
            return new Tuple<User, object>(null, null);
        }

    }
}