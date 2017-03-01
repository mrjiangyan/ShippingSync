using System.Collections.Generic;
using System.Web.Http;
using iPms.WebUtilities.Models;
using NextPms.Logic.ServiceDependency;
using NextPms.Util.IdGenerate;
using WebApi.Entity;
using NextPms.Dal.Entity.Statistics;
using NextPms.Cell.Config;

namespace iPms.WebUtilities.Controller
{
    public  class BaseController:ApiController
    {
        /// <summary>
        ///     请求相关的除请求参数以外的附加参数，包括AccessKey，UserToken，TimeStamp请数据。
        /// </summary>
        public RequestEntity RequestEntity { get; set; }


       
        private CurrentContextInfo _currentContextInfo;
        public ApiHeader Header = null;
        private static IIdGenerator _idGenerator;
 
        public IList<long> AppliedOrgs { get; set; }

       
        public CurrentContextInfo CurrentContextInfo
        {
            get { return _currentContextInfo ?? (_currentContextInfo = new CurrentContextInfo()); }
        }
      
       

        protected IIdGenerator IdGeneratorInstance
        {
            get
            {
                return _idGenerator ??
                       (_idGenerator = ServiceDependency.GetService<IIdGenerator>());
            }
            set { _idGenerator = value; }
        }

 

    }
}
