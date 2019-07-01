using BL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BusTicketBookingSystem.Controllers
{
    public class AdminAPIController : ApiController
    {
        AdminBL adminBL = new AdminBL();
        [HttpPost]
        public bool LoginValidate(AdminModel admin)
        {
            return adminBL.LoginValidate(admin);
        }

        [HttpGet]
        public List<BusModel> DisplayAllBusDetails(BusModel bus)
        {
            return adminBL.DisplayAllBusDetails(bus);
        }
        [HttpPost]
        public bool DeleteBusDetails(BusModel bus)
        {
            return adminBL.DeleteBusDetails(bus);
        }
        [HttpPost]
        public bool AddBusDetails(BusModel bus)
        {
            return adminBL.AddBusDetails(bus);
        }

        [HttpGet]
        public List<BusModel> UpdateBusDetails(int BusID)
        {
            return adminBL.UpdateBusDetails(BusID);
        }
    }
}
