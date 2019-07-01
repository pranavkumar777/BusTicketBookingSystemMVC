using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace DAL
{
    public class AdminDAL
    {

        string con = @"server=ASPIRE1251; database=bus; integrated Security=false; user=sa; password=aspire@123";
        SqlConnection sqlConnection = null;

        public bool LoginValidate(AdminModel admin)
        {
            try {
                sqlConnection = new SqlConnection(con);
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand("Select * from Admin where AdminEmail=@Email and AdminPassword=@Password", sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Email", admin.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", admin.Password);

                    DataTable dataTable = new DataTable();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataTable);
                    if (dataTable.Rows.Count > 0)
                    {
                        return true;
                    }
                    return false;
                }

            }
            catch(SqlException)
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        public List<BusModel> UpdateBusDetails(int BusID)
        {
            List<BusModel> busList = new List<BusModel>();
            try
            {   sqlConnection = new SqlConnection(con);
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("Select * from BusDetails where BusID=@busID", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@busID",BusID);
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    while(sqlDataReader.Read())
                    {
                        busList.Add(new BusModel
                        {
                            BusID = Convert.ToInt32(sqlDataReader["BusID"]),
                            BusTravelsName = sqlDataReader["BusTravelsName"].ToString(),
                            BusSource = sqlDataReader["BusSource"].ToString(),
                            BusDestination = sqlDataReader["BusDestination"].ToString(),
                            BusDepartureDate = Convert.ToDateTime(sqlDataReader["BusDepartureDate"]),
                            BusDepartureTime = (sqlDataReader["BusDepartureTime"]).ToString(),
                            BusSeatCount = Convert.ToInt32(sqlDataReader["BusSeatCount"]),
                            BusTicketCost = Convert.ToInt32(sqlDataReader["BusTicketCost"])
                        });

                    }
                    return busList;
                }

            }
            catch(SqlException)
            { }
            finally {
                sqlConnection.Close();
            }

            return busList;
        }
        public List<BusModel> DisplayAllBusDetails(BusModel bus)
        {
            List<BusModel> busDetails = new List<BusModel>();
            try
            {
                sqlConnection = new SqlConnection(con);
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("Select * from BusDetails", sqlConnection);
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    while(sqlDataReader.Read())
                    {
                        busDetails.Add(new BusModel

                        {
                            BusID = Convert.ToInt32(sqlDataReader["BusID"]),
                            BusTravelsName = sqlDataReader["BusTravelsName"].ToString(),
                            BusSource = sqlDataReader["BusSource"].ToString(),
                            BusDestination = sqlDataReader["BusDestination"].ToString(),
                            BusDepartureDate = Convert.ToDateTime(sqlDataReader["BusDepartureDate"]),
                            BusDepartureTime = (sqlDataReader["BusDepartureTime"]).ToString(),
                            BusSeatCount = Convert.ToInt32(sqlDataReader["BusSeatCount"]),
                            BusTicketCost = Convert.ToInt32(sqlDataReader["BusTicketCost"])

                        }
                        );
                    }

                }
               
            }
            catch(SqlException)
            {
            }
            finally { sqlConnection.Close(); }
            return busDetails;
        }



    /*    public bool DeleteBusDetails(BusModel bus)
        {
            sqlConnection = new SqlConnection(con);
            sqlConnection.Open();
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand("DeleteBusDetails", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@BusID", bus.BusID);
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
            catch
            {

            }
            finally {
                sqlConnection.Close();
            }
            return true;
        }
        */

        public bool DeleteBusDetails(BusModel bus)
        {
            sqlConnection = new SqlConnection(con);
            sqlConnection.Open();
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand("DeleteBusDetails", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@BusID", bus.BusID);
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
            catch(SqlException)
            {

            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }

        public bool AddBusDetails(BusModel bus)
        {
            try
            {
                sqlConnection = new SqlConnection(con);
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("AddBusDetails", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@BusTravelsName",bus.BusTravelsName );
                    sqlCommand.Parameters.AddWithValue("@BusSource",bus.BusSource );
                    sqlCommand.Parameters.AddWithValue("@BusDestination",bus.BusDestination );
                    sqlCommand.Parameters.AddWithValue("@BusDepartureDate", bus.BusDepartureDate);
                    sqlCommand.Parameters.AddWithValue("@BusDepartureTime", bus.BusDepartureTime);
                    sqlCommand.Parameters.AddWithValue("@BusSeatCount", bus.BusSeatCount);
                    sqlCommand.Parameters.AddWithValue("@BusTicketCost", bus.BusTicketCost);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch(SqlException)
            {
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }

       
    }
    }

