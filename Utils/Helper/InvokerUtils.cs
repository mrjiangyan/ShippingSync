using NextPms.Dal.Entity.Statistics;
using NextPms.Logic.ServiceDependency;
using System;
using System.Linq;
using System.Runtime.Caching;

namespace iPms.WebUtilities.Helper
{
    public static class InvokerUtils
    {

        private static readonly AppInvoker[] ArrayInvokers;
       
        static InvokerUtils()
        {
            using (var repertory = ServiceDependency.GetStatisticsEntityRepertory())
            {
                ArrayInvokers = repertory.AppInvokers.ToArray();
            }
        }


        

        public static AppInvoker GetDefaultAppInvoker()
        {
            return ArrayInvokers.FirstOrDefault(x => x.AccessKeyId == "123456");

        }

        public static AppInvoker GetInvoker(string accessKeyId)
        {
            //lock (LOCK)
            {
                if (string.IsNullOrEmpty(accessKeyId))
                    return GetDefaultAppInvoker();
                return ArrayInvokers.FirstOrDefault(x => x.AccessKeyId == accessKeyId);
            }


        }
    }
}
