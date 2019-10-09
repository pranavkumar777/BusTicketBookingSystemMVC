using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BusTicketBookingSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Default",
               url:"" ,
               defaults: new { controller = "Customer", action = "Login", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Login",
                url: "Customer/Login/{id}",
                defaults: new { controller = "Customer", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Signup",
                url: "Customer/Create/{id}",
                defaults: new { controller = "Customer", action = "Create", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Dashboard",
                url: "Customer/Dashboard/{id}",
                defaults: new { controller = "Customer", action = "Dashboard", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DisplaySeats",
                url: "Customer/DisplaySeats/{id}",
                defaults: new { controller = "Customer", action = "DisplaySeats", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Logout",
                url: "Customer/Logout/{id}",
                defaults: new { controller = "Customer", action = "Logout", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "Payment",
              url: "Customer/Payment/{id}",
              defaults: new { controller = "Customer", action = "Payment", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "BookingDetails",
              url: "Customer/BookingDetails/{id}",
              defaults: new { controller = "Customer", action = "BookingDetails", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "AdminDashboard",
              url: "Admin/AdminDashboard/{id}",
              defaults: new { controller = "Admin", action = "AdminDashboard", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "DeleteBusDetails",
              url: "Admin/Delete/{id}",
              defaults: new { controller = "Admin", action = "Delete", id = UrlParameter.Optional }
          );
            routes.MapRoute(
             name: "AddBusDetails",
             url: "Admin/AddBusDetails/{id}",
             defaults: new { controller = "Admin", action = "AddBusDetails", id = UrlParameter.Optional }
         );

        }
    }
}
