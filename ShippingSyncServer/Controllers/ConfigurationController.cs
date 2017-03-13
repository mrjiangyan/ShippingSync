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
    public class ConfigurationController : BaseApiController
    {
        [HttpPost]
        public async Task<ApiResponse<bool>> TryConnection(string connectionString)
        {
            return new ApiResponse<bool> { data = true };
        }

        /// <summary>
        /// 获取所有配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponse<bool>> Configuration()
        {
            return new ApiResponse<bool> { data = true };
        }

        /// <summary>
        /// 获取所有配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse<bool>> SaveConfiguration()
        {
            return new ApiResponse<bool> { data = true };
        }

    }
}
