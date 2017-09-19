using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SpongeSolutions.ServicoDireto.Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}.aspx/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },// Parameter defaults
                namespaces: new string[] { "SpongeSolutions.ServicoDireto.Admin.Controllers" }
                );

            routes.MapRoute("Customer WebSite",
                "{siteName}",
                new { controller = "Home", action = "Index", siteName = UrlParameter.Optional },
                namespaces: new string[] { "SpongeSolutions.ServicoDireto.Admin.Controllers" }
                );


            routes.MapRoute("Root", "", new { controller = "Home", action = "Index", siteName = "" });
        }
    }
}