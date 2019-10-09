using System;
using Model;
using System.Net.Http;

using System.Web.Mvc;
using System.Collections.Generic;
using System.Configuration;

namespace BusTicketBookingSystem.Controllers
{
    public class AdminController : Controller
    {

        public string URL = ConfigurationManager.AppSettings.Get("ConfigURL");
        /// <summary>
        /// Admin Dashboard page
        /// </summary>
        /// <returns>view</returns>
        public ActionResult AdminDashboard()
        {   
            return View();
        }

        /// <summary>
        /// Displays list of all Buses
        /// </summary>
        /// <param name="bus">object for BusModel</param>
        /// <returns>a list of buses</returns>
        [HttpGet]
        public ActionResult AdminDashboard(BusModel bus)
        {
            
                List<BusModel> busDetails = new List<BusModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    var get = client.GetAsync("AdminAPI/DisplayAllBusDetails");
                    get.Wait();
                    var result = get.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var getBusDetails = result.Content.ReadAsAsync<List<BusModel>>();
                        getBusDetails.Wait();
                        busDetails = getBusDetails.Result;
                    }
                }
            }
            catch (Exception exception)
            {
            }
            return View(busDetails);
        }

        
        /// <summary>
        /// Deletes Bus Details
        /// </summary>
        /// <param name="bus">object for BusModel</param>
        /// <returns>view</returns>
        public ActionResult Delete(BusModel bus)
        {
            try
            {
                using (var Client = new HttpClient())
                {
                    Client.BaseAddress = new Uri(URL);
                    var postTask = Client.PostAsJsonAsync<BusModel>("AdminAPI/DeleteBusDetails", bus);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        
                        return RedirectToRoute("AdminDashboard");
                    }
                }
            }
            catch (Exception exception)
            {
            }
            return View();  
        }

        /// <summary>
        /// Adds new BusDetails
        /// </summary>
        /// <param name="bus">object for BusModel</param>
        /// <returns>view</returns>
        [HttpPost]
        public ActionResult AddBusDetails(BusModel bus)
        {
            try
            {
                using (var Client = new HttpClient())
                {
                    Client.BaseAddress = new Uri(URL);
                    var postTask = Client.PostAsJsonAsync<BusModel>("AdminAPI/AddBusDetails", bus);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToRoute("AdminDashboard");
                    }
                }
            }
            catch (Exception exception)
            {
            }

                return View();
        }
        /// <summary>
        /// Bus Details registration page
        /// </summary>
        /// <returns>view</returns>
        public ActionResult AddBusDetails()
        {
            return View();
        }
        /// <summary>
        /// closes Admin session
        /// </summary>
        /// <returns>redirects to login</returns>
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToRoute("Login");
            
        }


    }
}