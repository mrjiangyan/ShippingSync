using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Utilities.Entity;

namespace ShippingSyncServer.Controllers
{
    public class LogController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<bool>> Login(string account, string password)
        {
            return new ApiResponse<bool> { data = true };
        }
    }
}
