using Model;
using System.Collections.Generic;

namespace DAL.Interface
{
    interface IAdminDatabaseOperations
    {
         
         List<BusModel> DisplayAllBusDetails(BusModel bus);
         bool DeleteBusDetails(BusModel bus);
         bool AddBusDetails(BusModel bus);
    }
}
