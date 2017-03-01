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
        static JsonHelper()
        {
            JsConfig<DateTime>.SerializeFn = t =>
                {
                    if (t.Kind != DateTimeKind.Utc)
                        return t.ToString(DateTimeFormat);
                    return t.ToString(DateTimeFormatUtc);
                };
            JsConfig<DateTime?>.SerializeFn = t =>
            {
                if (t.HasValue && t.Value.Kind != DateTimeKind.Utc)
                    return t.Value.ToString(DateTimeFormat);
                else if(t.HasValue)
                    return t.Value.ToString(DateTimeFormatUtc);
                return null;
            };

            JsConfig.IncludeNullValues = false;
            JsConfig.ExcludeTypeInfo = true;

            //JsConfig.AssumeUtc = true;
            JsConfig.IncludePublicFields = true;
            JsConfig<DateTime>.DeSerializeFn = s =>
            {
                DateTime dataTime;
                if (DateTime.TryParse(s, out dataTime))
                {
                    return dataTime;
                }
                return CurrentTime.Now;
            };
            JsConfig<DateTime?>.DeSerializeFn = s =>
            {
                DateTime dataTime;
                if (DateTime.TryParse(s, out dataTime))
                {
                    return dataTime;
                }
                return null;
            };
        }
        //private static readonly JsonSerializer JsonSerializer = new JsonSerializer
        //{
        //    NullValueHandling = NullValueHandling.Ignore,
        //    MissingMemberHandling = MissingMemberHandling.Ignore,
        //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        //};

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