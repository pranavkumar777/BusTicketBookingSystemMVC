using BL;
using Model;
using System.Collections.Generic;
using System.Web.Http;

namespace BusTicketBookingSystem.Controllers
{
    public class CustomerAPIController : ApiController
    {
        CustomerBL customerBL = new CustomerBL();
        [HttpPost]
        public bool AddNewCustomer(CustomerModel customer)
        {
             return customerBL.AddNewCustomer(customer);
          
        }

        [HttpPost]
        public bool LoginValidate(CustomerModel customer)
        {
            return customerBL.LoginValidate(customer);
        }

        [HttpPost]
        public List<BusModel> DisplayBusDetails(BusModel bus)
        {
            return customerBL.DisplayBusDetails(bus);
        }
        [HttpGet]
        public List<BusModel> DisplaySeats(int BusID)
        {
            return customerBL.DisplaySeats(BusID);
        }

        [HttpPost]
        public List<TicketModel> Payment(TicketModel ticket)
        {
            return customerBL.Payment(ticket);
        }
        [HttpGet]
        public List<TicketModel> BookingDetails(string CustomerEmail)
        {
            return customerBL.BookingDetails(CustomerEmail);
        }
    }
}
