using System;
using Model;
using System.Net.Http;

using System.Web.Mvc;
using System.Collections.Generic;

namespace BusTicketBookingSystem.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
                }
        [HttpPost]
        public ActionResult Login(AdminModel admin)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49278/api/AdminAPI/");
                var postTask = client.PostAsJsonAsync<AdminModel>("LoginValidate", admin);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Read = result.Content.ReadAsAsync<bool>();

                   

                    ViewBag.Result = Read.Result;
                    if (ViewBag.Result == true)
                    {
                        Session["AdminEmail"] = admin.Email;

                        return RedirectToAction("/AdminDashboard");
                    }
                }

            }

            return View();
        }

        public ActionResult AdminDashboard()
        {   

            return View();
        }

        [HttpGet]
        public ActionResult AdminDashboard(BusModel bus)
        {
            List<BusModel> busDetails = new List<BusModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49278/api/AdminAPI/");
                var get = client.GetAsync("DisplayAllBusDetails");
                get.Wait();
                var result = get.Result;
                if (result.IsSuccessStatusCode)
                {
                    var getBusDetails = result.Content.ReadAsAsync<List<BusModel>>();
                    getBusDetails.Wait();
                    busDetails = getBusDetails.Result;
                }
            }
            return View(busDetails);
        }
        public ActionResult UpdateBusDetails()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UpdateBusDetails(BusModel bus)
        {
            List<BusModel> busDetails = new List<BusModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49278/api/AdminAPI/");
                var get = client.GetAsync("UpdateBusDetails?BusID="+bus.BusID);
                get.Wait();
                var result = get.Result;
                if (result.IsSuccessStatusCode)
                {
                    var getBusDetails = result.Content.ReadAsAsync<List<BusModel>>();
                    getBusDetails.Wait();
                    busDetails = getBusDetails.Result;
                }
            }
            return View(busDetails);
        }
       
        public ActionResult Delete(BusModel bus)
        {
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri("http://localhost:49278/api/AdminAPI/");
                var postTask = Client.PostAsJsonAsync<BusModel>("DeleteBusDetails",bus);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("/AdminDashboard");
                }
            }
            return View();  
        }
        [HttpPost]
        public ActionResult AddBusDetails(BusModel bus)
        {
            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri("http://localhost:49278/api/AdminAPI/");
                var postTask = Client.PostAsJsonAsync<BusModel>("AddBusDetails", bus);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("/AdminDashboard");
                }
            }

                return View();
        }

        public ActionResult AddBusDetails()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("/Login");
            // return View();
        }


    }
}