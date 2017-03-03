
namespace Utilities.Entity
{

    public class ApiResponse<T>
    {
        public T data;

        /// <summary>
        /// 返回值 >=0 成功 <0 失败
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 返回信息，当Code < 0 时候，代表错误信息
        /// </summary>
        public string message { get; set; }


        public ApiResponse()
        {
            this.code = (int)System.Net.HttpStatusCode.OK;
                
        }


        public ApiResponse<T> WithError(System.Net.HttpStatusCode err)
        {
            this.code = (int)err;
            this.message = ApiError.GetErrorDesc(err);
            return this;
        }

        
        /// <summary>
        /// 异常信息，默认是
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ApiResponse<T> WithError(string message, int code = -100)
        {
            this.code = code;
            this.message = message;
            return this;
        }

    }

   
}
