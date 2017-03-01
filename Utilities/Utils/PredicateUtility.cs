using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Utils
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

           }
}
