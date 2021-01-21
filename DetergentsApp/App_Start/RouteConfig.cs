using System.Web.Mvc;
using System.Web.Routing;

namespace DetergentsApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "Public", action = "Public", id = UrlParameter.Optional}
            );
        }
    }
}