using DAL.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace DAL
{
    public class AdminDAL : IAdminDatabaseOperations
    {

        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString);
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
     
     
        /// <summary>
        /// This function displays the details of all buses
        /// </summary>
        /// <param name="bus">a object for BusModel</param>
        /// <returns>a list </returns>
        public List<BusModel> DisplayAllBusDetails(BusModel bus)
        {
            List<BusModel> busDetails = new List<BusModel>();
            try
            {
              
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("ViewAllBuses", sqlConnection)) { 
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                
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
            catch(SqlException sqlException)
            {
                log.Error(sqlException.ToString());
            }
            finally { sqlConnection.Close(); }
            return busDetails;
        }


        /// <summary>
        /// This functions deletes the busdetails
        /// </summary>
        /// <param name="bus">an object for BusModel</param>
        /// <returns>true or false</returns>

        public bool DeleteBusDetails(BusModel bus)
        {
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
            catch(SqlException sqlException)
            {
                log.Error(sqlException.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }
        /// <summary>
        /// This function adds details of new bus in the database
        /// </summary>
        /// <param name="bus">object for BusModel</param>
        /// <returns>returns true or false</returns>
        public bool AddBusDetails(BusModel bus)
        {
            try
            {
           
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
            catch(SqlException sqlException)
            {
                log.Error(sqlException.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }
    }
    }

