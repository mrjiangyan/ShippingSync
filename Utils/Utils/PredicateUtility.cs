using System;
using System.Collections.Generic;
using System.Linq;

namespace iPms.WebUtilities.Utils
{
    public static class PredicateUtility
    {
        public static String OrPredicate(IList<String> predicateList)
        {
            return "(" + String.Join("||", predicateList) + ")";
        }

        public static String AndPredicate(IList<String> predicateList)
        {
            return "(" + String.Join("&&", predicateList) + ")";
        }

        /// <summary>
        ///  销售模块的房价策略，市场活动和佣金策略，如果选择的列表超过一定的数量，会导致严重系统问题
        /// </summary>
        /// <param name="policyType">策略的类型，例如：房型，中介/公司级别</param>
        /// <param name="policyItemList">选择的列表，例如：房型列表，中介或协议公司列表等</param>
        public static void CheckPolicySelectedCountLimitation(string policyType, IEnumerable<string> policyItemList)
        {
            if (policyItemList != null && policyItemList.Count() > Utility.MaxAllowedPolicyItemCount)
            {
                throw new ArgumentException(string.Format(Utility.ExceedMaxPolicyItemCountErrorMsg, policyType));
            }
        }
    }
}
