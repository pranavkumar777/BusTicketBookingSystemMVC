

using DAL;
using Model;
using System.Collections.Generic;

namespace BL
{
    public class CustomerBL
    {

        CustomerDAL customerDAL = new CustomerDAL();
        public bool AddNewCustomer(CustomerModel customer)
        {
            return customerDAL.AddNewCustomer(customer);
           // return true;
        }
        public bool LoginValidate(CustomerModel customer)
        {
            return customerDAL.LoginValidate(customer);
        }

        
        public List<BusModel> DisplayBusDetails(BusModel bus)
        {
            return customerDAL.DisplayBusDetails(bus);
        }

        public List<BusModel> DisplaySeats(int BusID)
        {
            return customerDAL.DisplaySeats(BusID);
        }

        public List<TicketModel> Payment(TicketModel ticket)
        {
            ticket.SeatNumbers = ticket.seats[0].ToString();
            for (int i=1;i<ticket.seats.Length;i++)
            {
                ticket.SeatNumbers = ticket.seats[i].ToString()+","+ticket.SeatNumbers  ;
            }

            return customerDAL.Payment(ticket);
        }

        public List<TicketModel> BookingDetails(string CustomerEmail)
        {
            return customerDAL.BookingDetails(CustomerEmail);
        }
    }
}
