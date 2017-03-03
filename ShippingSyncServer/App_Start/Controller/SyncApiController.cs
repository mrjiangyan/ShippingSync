using ShippingSyncServer.Filters;
using System.Web.Http;

namespace ShippingSyncServer.Controller
{
    [DeflateCompression]
    [SyncAuthorize]
    public class SyncApiController : ApiController
    {
       
    }
}
