﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DomeApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "About",
                url: "about",
                defaults: new { controller = "Home", action = "About" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "contact",
                defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Comments",
                url: "comment/{action}/{postId}/{commentId}",
                defaults: new { controller = "Comment", action = "Index", postId = UrlParameter.Optional, commentId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "LandingPage",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "BlogPost", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}