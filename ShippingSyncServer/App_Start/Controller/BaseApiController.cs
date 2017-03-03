

using ShippingSyncServer.Filters;
using System.Web.Http;

namespace ShippingSyncServer
{
    [WebAuthorize]
    public class BaseApiController : ApiController
    {
        
    }
}