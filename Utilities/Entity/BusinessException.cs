using System;

namespace Utility.Entity
{
    public class ApiError
    {
        public static string GetErrorDesc(System.Net.HttpStatusCode code)
        {
            string rst = "未知错误";
            if (code == System.Net.HttpStatusCode.Unauthorized)
            {
                rst = "用户认证未通过,请重新登录";
            }
            else if (code == System.Net.HttpStatusCode.Forbidden)
            {
                rst = "数据校验失败，访问被阻止";
            }
            else if (code == System.Net.HttpStatusCode.BadRequest)
            {
                rst = "参数无效或者丢失";
            }
            else if (code == System.Net.HttpStatusCode.Conflict)
            {
                rst = "系统错误";
            }

            return rst;
        }
    }

   
}