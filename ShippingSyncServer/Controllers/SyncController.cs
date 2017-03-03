using ShippingSyncServer.Controller;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using Utilities.Entity;

namespace ShippingSyncServer.Controllers
{
    public class SyncController : SyncApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> HeartCheck()
        {
            return new ApiResponse<bool> { data=true };
        }

        // GET api/values/5
        
    }
}
