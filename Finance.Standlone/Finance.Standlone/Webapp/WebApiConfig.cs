using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Finance.Standlone.Webapp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute("AllCustomers", "customers", new { controller = "Customers", action = "Get" });
            config.Routes.MapHttpRoute("Login", "login", new { controller = "Login" });
        }
    }
}
