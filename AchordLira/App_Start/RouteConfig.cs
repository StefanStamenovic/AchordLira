﻿using System;
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
                name: "Artist",
                url: "Song/{artist}",
                defaults: new { controller = "Artist", action = "Index", artist = "" }
            );

            routes.MapRoute(
                name: "Song",
                url: "Song/{artist}/{name}",
                defaults: new { controller = "Song", action = "Index", artist = "", name = ""}
            );

            /*routes.MapRoute(
                name: "User",
                url: "User/{action}",
                defaults: new { controller = "User", action ="Index"}
                );*/

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
