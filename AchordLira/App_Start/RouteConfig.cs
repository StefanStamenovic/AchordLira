using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AchordLira
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "SongRequestCreate",
                url: "SongRequest/Create/",
                defaults: new { controller = "Home", action = "Create"}
            );

            routes.MapRoute(
                name: "SongRequestDelete",
                url: "SongRequest/Delete/",
                defaults: new { controller = "Home", action = "Delete" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{genre}",
                defaults: new { controller = "Home", action = "Index", genre = UrlParameter.Optional }
            );
        }
    }
}
