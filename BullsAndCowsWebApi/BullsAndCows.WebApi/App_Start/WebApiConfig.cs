using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Web.Http.Cors;

namespace BullsAndCows.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

           // config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            // Web API routes

            //config.Routes.MapHttpRoute(
            //    name: "notificationsApis",
            //    routeTemplate: "api/notifications/",
            //    defaults: new
            //    {
            //        controller = "Notifications",
            //        action = "Next"
            //    , });
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "guessApi",
                routeTemplate: "api/games/{gameId}/guess",
                defaults: new
                {
                    controller = "Games",
                    action = "Guess"
                , });

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}