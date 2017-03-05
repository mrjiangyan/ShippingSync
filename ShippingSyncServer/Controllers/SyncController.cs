using ShippingSyncServer.Controller;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using System.Web.Http;
using Utilities.Entity;

namespace ShippingSyncServer.Controllers
{
    public class SyncController : SyncApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponse<bool>> HeartCheck()
        {
            return new ApiResponse<bool> { data=true };
        }

        [HttpPost]
        public async Task<ApiResponse<bool>> Upload()
        {
            return new ApiResponse<bool> { data = true };
        }

        [HttpGet]
        public async Task<ApiResponse<bool>> Download()
        {
            return new ApiResponse<bool> { data = true };
        }

        [HttpPut]
        public async Task<ApiResponse<bool>> SyncFlag()
        {
            return new ApiResponse<bool> { data = true };
        }

    }
}
