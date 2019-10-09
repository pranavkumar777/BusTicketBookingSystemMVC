

using Model;
using System.Collections.Generic;

namespace DAL
{
    interface ICustomerDataBaseOperations
    {
        bool AddNewCustomer(CustomerModel customer);
        List<CustomerModel> LoginValidate(CustomerModel customer);
        List<BusModel> DisplayBusDetails(BusModel bus);
        List<BusModel> DisplaySeats(int BusID);
        List<TicketModel> Payment(TicketModel ticket);
        List<TicketModel> BookingDetails(string CustomerEmail);

    

    }
}
