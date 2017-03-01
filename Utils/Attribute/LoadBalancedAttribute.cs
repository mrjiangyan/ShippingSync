using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using iPms.Interface;
using iPms.WebUtilities.Controller;
using iPms.WebUtilities.Extensions;
using NextPms.Dal.Entity.App;
using NextPms.Dal.Entity.BusinessEnum;
using NextPms.Dal.Util;
using NextPms.Logic.ServiceDependency;
using NextPms.Util.Time;

namespace iPms.WebUtilities.Attribute
{
    public class LoadBalancedAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {//暂时只用取余算法实现，但添加或删除服务器造成区间不稳定的补偿措施是？
            var apiRoute = Path.Combine(
                actionContext.ControllerContext.ControllerDescriptor.ControllerName,
                actionContext.ActionDescriptor.ActionName);
            var ip = actionContext.Request.GetUserHostAddress();
            int clientsCount;
            var clientIndex = 0;

            using (var appRep = ServiceDependency.GetAppEntityRepertory())
            {
                var client = appRep.ApiLoadBalanceConfigs.SingleOrDefault(c => !c.IsOuter && c.ApiRoute == apiRoute && c.Client == ip);
                if (client == null)
                {
                    appRep.ApiLoadBalanceConfigs.Add(new ApiLoadBalanceConfig
                    {
                        OwnerId = -1,
                        ApiRoute = apiRoute,
                        Client = ip,
                        CreateTimeInUtc = CurrentTime.UtcNow,
                        CreatorName = "Ota连接组",
                        IsAlive = true,
                        IsValid = true
                    });
                }
                else
                {
                    if (!client.IsAlive)
                    {
                        client = client.WrapForChangeTrack();
                        client.IsAlive = true;
                        appRep.ApiLoadBalanceConfigs.Update(client);
                    }
                }

                var clients =
                    appRep.ApiLoadBalanceConfigs.Where(c => !c.IsOuter && c.ApiRoute == apiRoute && c.IsAlive).OrderBy(c => c.CreateTimeInUtc)
                    .Select(c => c.Client).ToArray();
                clientsCount = clients.Any() ? clients.Length : 1;
                for (var i = 0; i < clients.Length; i++)
                {
                    if (clients[i] == ip)
                    {
                        clientIndex = i;
                    }
                }
            }

            long[] orgs;
            using(var context=ServiceDependency.GetEntityContext())
            using (var repertory = context.PinnedEntityRepertory)
            {
                orgs =
                    repertory.Orgs.Where(o => o.IsActive && o.OrgTypeId == (byte)OrgType.Hotel && o.OrgId % clientsCount == clientIndex)
                        .Select(o => o.OrgId)
                        .ToArray()
                        .Distinct()
                        .ToArray();
            }

            ((OtaApiController)actionContext.ControllerContext.Controller).AppliedOrgs = orgs;
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}
