using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace iPms.WebUtilities.Utils
{
    public class Utility
    {
        public static readonly Dictionary<string, string> PromotionPolicyTypeDictionary = new Dictionary<string, string>()
        {
            { "OrderSource", "订单来源" },
            { "RoomTypeCountDetails",  "房型"},
            { "CheckinType", "入住类别" }, 
            { "CustomerCategory", "客源类别" },
            { "PriceStakeholder.Level", "会员级别" }, 
            { "Order.CreateTimeInUtc", "预订下单时间" }, 
            { "Order.EstimatedArriveTime", "入住时间" }, 
            { "Days", "入住天数" }, 
            { "AdvanceDays", "预订提前天数" },
            { "TotalRoomCount", "预订间数" }
        };

        public static readonly Dictionary<string, string> PricePolicyTypeDictionary = new Dictionary<string, string>()
        {
            { "OrderSource", "订单来源" },
            { "CheckinTypeId", "入住类别" },
            { "PriceRoomTypeId",  "房型"},
            { "CustomerCategoryId", "客源类别" },
            { "PriceStakeholder.Level", "会员级别" }, 
            { "Date", "入住时间" }
        };

        public const int MaxAllowedPolicyItemCount = 260;
        public const string ExceedMaxPolicyItemCountErrorMsg = "选择的 {0} 数量超过上限";

        public const string Points = "积分";
        public const string Create = "新建";
        public const string Upgrade = "升级";

        public static TimeSpan? GetTimeSpan(string boundry)
        {
            TimeSpan boundryTime;

            return TimeSpan.TryParse(boundry, out boundryTime)
                       ? new TimeSpan?(boundryTime)
                       : null;
        }

        public static string WordToNumber(string word)
        {
            string e = "([零一二三四五六七八九十百千万亿])+";
            MatchCollection mc = Regex.Matches(word, e);

            foreach (Match m in mc)
            {
                word = word.Replace(m.Value, GetNumber4Chinese(m.Value));
            }
            return word;
        }

        public static string GetNumber4Chinese(string str)
        {
            string istr = "0";
            string cstr = string.Empty;
            string nstr = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                cstr = str.Substring(i, 1);
                nstr = (i < str.Length - 1) ? str.Substring(i + 1, 1) : "个";

                switch (cstr)
                {
                    case "一":
                    case "壹":
                        istr = (Convert.ToInt64(istr) + 1 * GetBaseNumber4ChineseUnit(nstr)).ToString();
                        break;
                    case "二":
                    case "两":
                    case "贰":
                        istr = (Convert.ToInt64(istr) + 2 * GetBaseNumber4ChineseUnit(nstr)).ToString();
                        break;
                    case "三":
                    case "叁":
                        istr = (Convert.ToInt64(istr) + 3 * GetBaseNumber4ChineseUnit(nstr)).ToString();
                        break;
                    case "四":
                    case "肆":
                        istr = (Convert.ToInt64(istr) + 4 * GetBaseNumber4ChineseUnit(nstr)).ToString();
                        break;
                    case "五":
                    case "伍":
                        istr = (Convert.ToInt64(istr) + 5 * GetBaseNumber4ChineseUnit(nstr)).ToString();
                        break;
                    case "六":
                    case "陆":
                        istr = (Convert.ToInt64(istr) + 6 * GetBaseNumber4ChineseUnit(nstr)).ToString();
                        break;
                    case "七":
                    case "柒":
                        istr = (Convert.ToInt64(istr) + 7 * GetBaseNumber4ChineseUnit(nstr)).ToString();
                        break;
                    case "八":
                    case "捌":
                        istr = (Convert.ToInt64(istr) + 8 * GetBaseNumber4ChineseUnit(nstr)).ToString();
                        break;
                    case "九":
                    case "玖":
                        istr = (Convert.ToInt64(istr) + 9 * GetBaseNumber4ChineseUnit(nstr)).ToString();
                        break;
                    default:
                        if (str.Length < 2)
                        {
                            istr = GetBaseNumber4ChineseUnit(cstr).ToString();
                        }
                        else if (str.Length == 2 && (str.StartsWith("十") || str.StartsWith("拾")))
                        {
                            return (10 + Convert.ToInt64(GetNumber4Chinese(nstr))).ToString();
                        }
                        break;
                }
            }
            return istr;
        }

        private static int GetBaseNumber4ChineseUnit(string str)
        {
            var ibase = 0;
            for (var i = 0; i < str.Length; i++)
            {
                var cstr = str.Substring(i, 1);
                if (cstr == "万")
                {
                    ibase = 10000;
                }
                else if (cstr == "千")
                {
                    ibase = 1000;
                }
                else if (cstr == "百")
                {
                    ibase = 100;
                }
                else if (cstr == "十")
                {
                    ibase = 10;
                }
                else if (cstr == "个")
                {
                    ibase = 1;
                }
            }
            return ibase;
        }

        public const string FieldStringValueTooLongKeyword = "String or binary data would be truncated.";
        public const string FieldStringValueTooLongErrorMsg = "相关内容的长度可能过长";
        public const string DbUnhandledExceptionMsg = "发生数据库异常";
        public const string SystemUnhandledExceptionMsg = "发生系统异常";
        public const string FaultExceptionMsg = "发生Fault异常";
    }
}
