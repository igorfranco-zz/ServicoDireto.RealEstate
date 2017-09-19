using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SpongeSolutions.ServicoDireto.Admin
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api
            //// Web API configuration and services
            config.EnableCors();
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;            

            EnableCrossSiteRequests(config);           

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        private static void EnableCrossSiteRequests(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute(
                origins: "*",
                headers: "*",
                methods: "*");
            config.EnableCors(cors);
        }
    }
}
