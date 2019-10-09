using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Web.Mvc;

namespace BusTicketBookingSystem.Controllers
{
    public class CustomerController : Controller
    {

        public string URL = ConfigurationManager.AppSettings.Get("ConfigURL");

        /// <summary>
        /// Customer registration page
        /// </summary>
        /// <returns>view</returns>
        public ActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Registers new customer details
        /// </summary>
        /// <param name="customer">object for CustomerModel</param>
        /// <returns>view</returns>
        [HttpPost]
        public ActionResult Create(CustomerModel customer)
        {
            try
            {

                {
                    if (customer.CustomerPassword == customer.CustomerConfirmPassword)
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(URL);
                            var postTask = client.PostAsJsonAsync<CustomerModel>("CustomerAPI/AddNewCustomer", customer);
                            postTask.Wait();

                            var result = postTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                var Read = result.Content.ReadAsAsync<bool>();
                                ViewBag.Result = Read.Result;
                                if (ViewBag.Result == false)
                                {
                                    ViewBag.Message = "Email already exists";
                                    return View();
                                }
                                return RedirectToRoute("Login");
                            }
                        }
                    }

                    else
                    {
                        ViewBag.Result = "Password do not match";

                    }
                }
            }
            catch (Exception exception)
            {
            }
            return View();
        }

        /// <summary>
        /// login page view
        /// </summary>
        /// <returns>view</returns>
        public ActionResult Login()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult Login(CustomerModel customer)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.PostAsJsonAsync<CustomerModel>("CustomerAPI/LoginValidate", customer);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var Read = result.Content.ReadAsAsync<string>();
                        ViewBag.Result = Read.Result;

                        if (ViewBag.Result == "customer")
                        {
                            Session["CustomerEmail"] = customer.CustomerEmail;
                            return RedirectToRoute("Dashboard");

                        }
                        else if (ViewBag.Result == "admin")
                        {
                            Session["CustomerEmail"] = customer.CustomerEmail;
                            return RedirectToRoute("AdminDashboard");
                        }

                        else
                        {
                            ViewBag.Message = "Invalid email or password";
                            return View();
                        }
                    }

                }
            }
            catch (Exception exception)
            {
            }

            return View();
        }


        /// <summary>
        /// Dashboard page view
        /// </summary>
        /// <returns>view</returns>
        public ActionResult Dashboard()
        {
            return View();
        }

        /// <summary>
        /// Displays the results searched by customer
        /// </summary>
        /// <param name="bus">object for BusModel</param>
        /// <returns>view</returns>
        [HttpPost]
        public ActionResult Dashboard(BusModel bus)
        {
            List<BusModel> busList = new List<BusModel>();
            try
            {

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(URL);
                    var postTask = client.PostAsJsonAsync<BusModel>("CustomerAPI/DisplayBusDetails", bus);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var postResponse = result.Content.ReadAsAsync<List<BusModel>>();
                        busList = postResponse.Result;
                    }

                }
            }
            catch (Exception exception)
            {
            }
            return View(busList);
        }
      /// <summary>
      /// Display seats for customer to book
      /// </summary>
      /// <param name="bus">object for BusModel</param>
      /// <returns>view</returns>
        public ActionResult DisplaySeats(BusModel bus)
        {
            List<BusModel> seatList = new List<BusModel>();
            try
            {
                var state = Session["CustomerEmail"];
                if (state == null)
                {
                    return RedirectToRoute("Login");
                }
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    var get = client.GetAsync("CustomerAPI/DisplaySeats?BusID=" + bus.BusID);
                    get.Wait();
                    var result = get.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var getSeats = result.Content.ReadAsAsync<List<BusModel>>();
                        getSeats.Wait();
                        seatList = getSeats.Result;
                    }
                }
            }
            catch (Exception exception)
            {             
            }
            return View(seatList);
        }

       /// <summary>
       /// removes user session
       /// </summary>
       /// <returns>login page</returns>

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToRoute("Login");
         
        }
        /// <summary>
        /// payment page view
        /// </summary>
        /// <returns>view</returns>
        public ActionResult Payment()
        {
            return View();
        }

        /// <summary>
        /// books tickets for customer
        /// </summary>
        /// <param name="ticket">object for TicketModel</param>
        /// <returns>view</returns>
        [HttpPost]
        public ActionResult Payment(TicketModel ticket)
        {

            List<TicketModel> ticketDetails = new List<TicketModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    var postTask = client.PostAsJsonAsync<TicketModel>("CustomerAPI/Payment", ticket);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var postResponse = result.Content.ReadAsAsync<List<TicketModel>>();
                        ticketDetails = postResponse.Result;

                    }
                }
            }
            catch (Exception exception)
            {

            }
            return View(ticketDetails);
        }
        
        /// <summary>
        /// displays booking details of customer
        /// </summary>
        /// <param name="ticket">object for TicketModel</param>
        /// <returns>view</returns>
        public ActionResult BookingDetails(TicketModel ticket)
        {
            List<TicketModel> ticketDetails = new List<TicketModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    var get = client.GetAsync("CustomerAPI/BookingDetails?CustomerEmail=" + ticket.CustomerEmail);
                    get.Wait();
                    var result = get.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var getBookingDetails = result.Content.ReadAsAsync<List<TicketModel>>();
                        getBookingDetails.Wait();
                        ticketDetails = getBookingDetails.Result;
                    }
                }
            }
            catch (Exception exception)
            {

            }
            return View(ticketDetails);
        }
        
    }
}