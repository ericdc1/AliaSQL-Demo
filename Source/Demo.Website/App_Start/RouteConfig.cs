using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Demo.Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //for stackexchange.exceptional
            routes.MapRoute(
                name: "Exceptions",
                url: "{controller}/{action}/{resource}/{subResource}",
                defaults:
                    new
                    {
                        controller = "Error",
                        action = "Exceptions",
                        resource = UrlParameter.Optional,
                        subResource = UrlParameter.Optional
                    }
                );
        }
    }
}