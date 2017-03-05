
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using ShippingSyncServer.Filters;
using Newtonsoft.Json.Serialization;

namespace ShippingSyncServer
{
    public static class WebApiConfig
    {
      
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new { action = UrlParameter.Optional, id = UrlParameter.Optional }
                );
        }

        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}"
                , new { id = RouteParameter.Optional }
                );
            config.MessageHandlers.Add(new MessageLoggingHandler());
            config.Formatters.Insert(0, GlobalConfigurationHelper.Init());
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Filters.Add(new SyncExceptionFilterAttribute());
            // Convert all dates to UTC
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }


     
       
    }
}
