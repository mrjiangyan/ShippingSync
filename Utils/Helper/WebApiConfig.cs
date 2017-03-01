using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using iPms.WebUtilities.Attribute;
using iPms.WebUtilities.Binder;
using iPms.WebUtilities.MessageHandlers;
using NextPms.Util.Time;
using ServiceStack;
using ServiceStack.Text;
using NextPms.Util;

namespace iPms.WebUtilities.Helper
{
    public static class WebApiConfig
    {
        private const string DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz";

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
          
            
            config.Routes.MapHttpRoute(
                "ReportApi",
                "report/{controller}/{id}",
                new { id = RouteParameter.Optional },
                new[] { "iPms.WebApi.Controllers.Report" }
            );

           
            Licensing.RegisterLicense(
            @"1985-e1JlZjoxOTg1LE5hbWU6QmV5b25kSG9zdCxUeXBlOlJlZGlzQnVzaW5lc3MsSGFzaDpzYS94NTRYMngwam5lWm0zaHdDTWI1S2svNDl1QXk3K2xIcFp1bVdXNmlUVXRlQTBvWWJKSU90R2JFQXI3QnpmTUxuTW9SWlZRNEtTMmwzcEhiTmhTRDNleVR1aTBvZ1U2TndBVitoYU9Qb0U2b2NoNEIvb09qb1cvSWJLcnFQZEp1d0RiNTFNemVGUkdRa0R6Q0ZJMExDSU5aZ1VIdzBQKzBRd2xwTDJtZDA9LEV4cGlyeToyMDE1LTEwLTMxfQ==");
            config.MessageHandlers.Add(new MessageLoggingHandler());
            config.Formatters.Insert(0, GlobalConfigurationHelper.Init());
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Filters.Add(new PmsExceptionFilterAttribute());
            InitJsonDateFormat();
        }


        private static void InitJsonDateFormat()
        {
            JsConfig<DateTime>.SerializeFn = t =>
                {
                    if (t.Kind != DateTimeKind.Utc)
                        return t.ToString(JsonHelper.DateTimeFormat);
                    
                    return t.ToString(JsonHelper.DateTimeFormatUtc);
                   
                };
            JsConfig<DateTime?>.SerializeFn = t =>
            {
                if (t.HasValue && t.Value.Kind != DateTimeKind.Utc)
                    return t.Value.ToString(JsonHelper.DateTimeFormat);
                else if (t.HasValue)
                    return t.Value.ToString(JsonHelper.DateTimeFormatUtc);
                return null;
            };

            JsConfig.IncludeNullValues = false;
            JsConfig.ExcludeTypeInfo = true;

            //JsConfig.AssumeUtc = true;
            JsConfig.IncludePublicFields = true;
            JsConfig.TreatEnumAsInteger = true;
            JsConfig<DateTime>.DeSerializeFn = s =>
            {
                DateTime dataTime;
                return DateTime.TryParse(s, out dataTime) ? dataTime : CurrentTime.Now;
            };
            JsConfig<DateTime?>.DeSerializeFn = s =>
            {
                DateTime dataTime;
                if (DateTime.TryParse(s, out dataTime))
                {
                    return dataTime;
                }
                return null;
            };
        }

       
    }
}
