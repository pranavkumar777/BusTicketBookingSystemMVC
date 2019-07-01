

using DAL;
using Model;
using System.Collections.Generic;

namespace BL
{
    public class AdminBL
    {
        AdminDAL adminDAL = new AdminDAL();
        public bool LoginValidate(AdminModel admin)
        {
            return adminDAL.LoginValidate(admin);
        }

        public List<BusModel> DisplayAllBusDetails(BusModel bus)
        {
            return adminDAL.DisplayAllBusDetails(bus);
        }

        public bool DeleteBusDetails(BusModel bus)
        {
            return adminDAL.DeleteBusDetails(bus);
        }
        public bool AddBusDetails(BusModel bus)
        {
            return adminDAL.AddBusDetails(bus);
        }
        public List<BusModel> UpdateBusDetails(int BusID)
        {

            return adminDAL.UpdateBusDetails(BusID);
        }

    }
}
