using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace iPms.WebUtilities.Helper
{
    public static class HttpRouteCollectionEx
    {
        public static IHttpRoute MapHttpRoute(this HttpRouteCollection routes, string name, string routeTemplate, object defaults, string[] namespaces)
        {
            return routes.MapHttpRoute(name, routeTemplate, defaults, null, null, namespaces);
        }
        public static IHttpRoute MapHttpRoute(this HttpRouteCollection routes, string name, string routeTemplate, object defaults, object constraints, HttpMessageHandler handler, string[] namespaces)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            var routeValue = new HttpRouteValueDictionary(new { Namespace = namespaces });//设置路由值
            var route = routes.CreateRoute(routeTemplate, new HttpRouteValueDictionary(defaults), new HttpRouteValueDictionary(constraints), routeValue, handler);
            routes.Add(name, route);
            return route;
        }
    }
}
