using Newtonsoft.Json;
using System;

namespace iPms.WebUtilities.Helper
{
    /// <summary>
    /// Json字符串序列化和反序列化库
    /// </summary>
    public static class JsonHelper
    {
        public  const string DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz";
        public const string DateTimeFormatUtc = "yyyy'-'MM'-'dd'T'HH':'mm':'ss+08:00";
    
        /// <summary>
        /// 将对象进行序列化
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string JsonObjectSerialize(this object source)
        {
           return JsonConvert.SerializeObject(source);
               
        }


        /// <summary>
        /// 将Json字符串进行反序列化为目标对象类型
        /// </summary>
        /// <param name="jsonString">原始的Json字符串</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object JsonObjectDeserialize(this string jsonString,Type type)
        {
            return JsonConvert.DeserializeObject(jsonString,type);
        }

       

        /// <summary>
        /// 将Json字符串进行反序列化为目标对象类型
        /// </summary>
        /// <param name="jsonString">原始的Json字符串</param>
        /// <returns></returns>
        public static T JsonObjectDeserialize<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
           
        }


    }
}