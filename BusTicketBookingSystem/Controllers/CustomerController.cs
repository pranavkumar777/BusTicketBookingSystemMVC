using Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;

namespace BusTicketBookingSystem.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                if (customer.CustomerPassword == customer.CustomerConfirmPassword)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:49278/api/CustomerAPI/");
                        var postTask = client.PostAsJsonAsync<CustomerModel>("AddNewCustomer", customer);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("/Login");

                        }
                    }
                }

                else
                {
                    ViewBag.Result = "Password do not match";

                }
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(CustomerModel customer)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49278/api/CustomerAPI/");
                var postTask = client.PostAsJsonAsync<CustomerModel>("LoginValidate", customer);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Read = result.Content.ReadAsAsync<bool>();

                    //   return RedirectToAction("/Create");

                    ViewBag.Result = Read.Result;
                    if (ViewBag.Result == true)
                    {
                        Session["CustomerEmail"] = customer.CustomerEmail;
                        
                        return RedirectToAction("/Dashboard");
                    }
                }

            }


            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dashboard(BusModel bus)
        {
            List<BusModel> busList = new List<BusModel>();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:49278/api/CustomerAPI/");
                var postTask = client.PostAsJsonAsync<BusModel>("DisplayBusDetails", bus);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var postResponse = result.Content.ReadAsAsync<List<BusModel>>();
                    busList = postResponse.Result;
                    //  return RedirectToAction("/BusList",busList);

                }

            }
            return View(busList);
        }
        /*
                public ActionResult SeatSelection()
                {
                    return View();
                } */

        public ActionResult DisplaySeats(BusModel bus)
        {
            List<BusModel> seatList = new List<BusModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49278/api/CustomerAPI/");
                var get = client.GetAsync("DisplaySeats?BusID=" + bus.BusID);
                get.Wait();
                var result = get.Result;
                if (result.IsSuccessStatusCode)
                {
                    var getSeats = result.Content.ReadAsAsync<List<BusModel>>();
                    getSeats.Wait();
                    seatList = getSeats.Result;
                }
            }
            return View(seatList);
        }

        public ActionResult BusList(BusModel busList)
        {
            return View(busList);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("/Login");
            // return View();
        }

        public ActionResult Payment()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Payment(TicketModel ticket)
        {

            List<TicketModel> ticketDetails = new List<TicketModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49278/api/CustomerAPI/");
                var postTask = client.PostAsJsonAsync<TicketModel>("Payment", ticket);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var postResponse = result.Content.ReadAsAsync<List<TicketModel>>();
                    ticketDetails = postResponse.Result;           

                }
                return View(ticketDetails);
            }
        }
        
        public ActionResult BookingDetails(TicketModel ticket)
        {
            List<TicketModel> ticketDetails = new List<TicketModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49278/api/CustomerAPI/");
                var get = client.GetAsync("BookingDetails?CustomerEmail=" + ticket.CustomerEmail);
                get.Wait();
                var result = get.Result;
                if (result.IsSuccessStatusCode)
                {
                    var getBookingDetails = result.Content.ReadAsAsync<List<TicketModel>>();
                    getBookingDetails.Wait();
                    ticketDetails = getBookingDetails.Result;
                }
            }
            return View(ticketDetails);
        }
        
    }
}