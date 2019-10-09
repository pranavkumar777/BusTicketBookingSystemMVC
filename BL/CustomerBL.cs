

using DAL;
using Model;
using System;
using System.Collections.Generic;


namespace BL
{
    public class CustomerBL
    {

        CustomerDAL customerDAL = new CustomerDAL();
        public bool AddNewCustomer(CustomerModel customer)
        {

            if (customerDAL.IfEmployeeIdExists(customer.CustomerEmail))
            {
                return false;
            }
            return customerDAL.AddNewCustomer(customer);   
                 
        }

        public string LoginValidate(CustomerModel customer)
        {
            string role = "";
            List<CustomerModel>Details = customerDAL.LoginValidate(customer);
            foreach (var item in Details)
            {
                role = item.CustomerRole;
            }
          


            return role;
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
