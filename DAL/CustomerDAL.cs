using Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;

using System.Configuration;


namespace DAL
{
    public class CustomerDAL : ICustomerDataBaseOperations
    {
    
          SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString);
          private static readonly log4net.ILog log =log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This function adds new customer details to the database
        /// </summary>
        /// <param name="customer">object for customer model</param>
        /// <returns>true or false</returns>
        public bool AddNewCustomer(CustomerModel customer)
        {
            try
            {
                
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("AddCustomerDetails", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                    sqlCommand.Parameters.AddWithValue("@CustomerEmail", customer.CustomerEmail);
                    sqlCommand.Parameters.AddWithValue("@CustomerMobileNumber", customer.CustomerMobileNumber);
                    sqlCommand.Parameters.AddWithValue("@CustomerPassword", customer.CustomerPassword);


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

        /// <summary>
        /// This function authenticates user login credentials
        /// </summary>
        /// <param name="customer">object for customer model</param>
        /// <returns>true or false</returns>
        public List<CustomerModel> LoginValidate(CustomerModel customer)
        {
            List<CustomerModel> Details = new List<CustomerModel>();
            sqlConnection.Open();
            try
            {
               
                log.Info("Application is working");
                using (SqlCommand sqlCommand = new SqlCommand("CustomerAuthenticate", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@CustomerEmail", customer.CustomerEmail);
                    sqlCommand.Parameters.AddWithValue("@CustomerPassword", customer.CustomerPassword);

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Details.Add(new CustomerModel
                        {
                            CustomerRole = sqlDataReader["role"].ToString()
                        });
                        return Details;
                    }
           
                    return Details;
                
                }
            }
            catch(SqlException sqlException)
            {
                log.Error(sqlException.ToString());
                return Details;
            }
            finally
            {
                sqlConnection.Close();
                
            }

        }

        /// <summary>
        /// This function displays the details of buses which customer searched
        /// </summary>
        /// <param name="bus">object for Bus model</param>
        /// <returns>a list</returns>
        public List<BusModel> DisplayBusDetails(BusModel bus)
        {
            List<BusModel> BusDetails = new List<BusModel>();
            try
            {           
                  sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("DisplaySearchedBuses", sqlConnection))
                { 
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@source", bus.BusSource);
                sqlCommand.Parameters.AddWithValue("@destination", bus.BusDestination);
                sqlCommand.Parameters.AddWithValue("@busdate", bus.BusDepartureDate);

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                
                    while (sqlDataReader.Read())
                    {
                        BusDetails.Add(new BusModel

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
            catch (SqlException sqlException)
            {
                log.Error(sqlException.ToString());

            }

            finally { sqlConnection.Close(); }

            return BusDetails;
        }
        
        /// <summary>
        /// This function displays the booked seats and empty seats
        /// </summary>
        /// <param name="BusID">unique id of each bus</param>
        /// <returns>a list</returns>
               public List<BusModel> DisplaySeats(int BusID)
                {
                    List<BusModel> seatList = new List<BusModel>();
           
                    sqlConnection.Open();

                    try
                    {


                        using (SqlCommand sqlCommand = new SqlCommand("PartiallyBookedBusSeats", sqlConnection))
                        {
                          sqlCommand.CommandType = CommandType.StoredProcedure;
                             sqlCommand.Parameters.AddWithValue("@BusID", BusID);
                            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                            while (sqlDataReader.Read())
                            {
                                seatList.Add(new BusModel
                                {
                                    BusID = Convert.ToInt32(sqlDataReader["BusID"]),
                                    BusSeatCount = Convert.ToInt32(sqlDataReader["SeatNumber"]),
                                    BusTicketCost = Convert.ToInt32(sqlDataReader["BusTicketCost"]),
                                     BusSource = sqlDataReader["BusSource"].ToString(),
                                    BusDestination = sqlDataReader["BusDestination"].ToString()
                                   
                                });
                            }
                            sqlConnection.Close();
                        }
                        if(seatList.Count==0)
                        {
                            using (SqlCommand sqlCommand = new SqlCommand("EmptyBusSeats ", sqlConnection))
                            {
                                sqlConnection.Open();
                                sqlCommand.CommandType = CommandType.StoredProcedure;
                                sqlCommand.Parameters.AddWithValue("@BusId", BusID);
                                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                                while (sqlDataReader.Read())
                                {
                                    seatList.Add(new BusModel
                                    {
                                        BusID = Convert.ToInt32(sqlDataReader["BusID"]),                              
                                        BusTicketCost = Convert.ToInt32(sqlDataReader["BusTicketCost"]),
                                        BusSource = sqlDataReader["BusSource"].ToString(),
                                        BusDestination = sqlDataReader["BusDestination"].ToString()
                                       
                                    });
                                }
                            }

                        }

                    }
                    catch (SqlException sqlException)
                    {
                log.Error(sqlException.ToString());
                       }
                    finally
                    {
                        sqlConnection.Close();
                    }
                    return seatList;
                }

        /// <summary>
        /// This functions makes payment and registers customer's ticket
        /// </summary>
        /// <param name="ticket">object for ticketmodel </param>
        /// <returns>a list </returns>
        public List<TicketModel> Payment(TicketModel ticket)
        {
            List<TicketModel> ticketDetails = new List<TicketModel>();      
            try
            {
                sqlConnection.Open();
                int seatCount = ticket.seats.Length;
                int totalCost = seatCount * ticket.Cost;

                using (SqlCommand sqlCommand = new SqlCommand("AddPaymentDetails", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@id", ticket.BusID);
                    sqlCommand.Parameters.AddWithValue("@Email", ticket.CustomerEmail);
                    sqlCommand.Parameters.AddWithValue("@seat", seatCount);
                    sqlCommand.Parameters.AddWithValue("@totalcost", totalCost);
                    sqlCommand.Parameters.AddWithValue("@BusSource", ticket.BusSource);
                    sqlCommand.Parameters.AddWithValue("@BusDestination", ticket.BusDestination);
                    sqlCommand.Parameters.AddWithValue("@seatnumbers", ticket.SeatNumbers);                   
                    sqlCommand.ExecuteNonQuery();
                }
                for (int i = 0; i < seatCount; i++)
                    using (SqlCommand sqlCommand = new SqlCommand("AddSeatDetails", sqlConnection))
                    {
                        
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@BusID", ticket.BusID);
                        sqlCommand.Parameters.AddWithValue("@Cost", ticket.Cost);
                        sqlCommand.Parameters.AddWithValue("@Bussource", ticket.BusSource);
                        sqlCommand.Parameters.AddWithValue("@Busdestination", ticket.BusDestination);
                        sqlCommand.Parameters.AddWithValue("@SeatNumber", ticket.seats[i]);

                        sqlCommand.ExecuteNonQuery();


                    }

                ticketDetails.Add(new TicketModel
                {
                    BusID =ticket.BusID,
                    CustomerEmail = ticket.CustomerEmail,
                    NumberOfSeats = seatCount, 
                    Cost = ticket.Cost,
                    BusSource = ticket.BusSource,
                    BusDestination = ticket.BusDestination
                });
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }

            return ticketDetails;
        }
       
        /// <summary>
        /// This function displays  booking details of customer
        /// </summary>
        /// <param name="CustomerEmail">email id of each customer</param>
        /// <returns>a list </returns>
        public List<TicketModel> BookingDetails(string CustomerEmail)
        {
            List<TicketModel> ticketDetails = new List<TicketModel>();
            sqlConnection.Open();

            try
            {

                using (SqlCommand sqlCommand = new SqlCommand("ViewBookingDetailsByCustomer", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@email", CustomerEmail);
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        ticketDetails.Add(new TicketModel
                        {
                            BusID = Convert.ToInt32(sqlDataReader["BusID"]),
                            CustomerEmail = sqlDataReader["CustomerEmail"].ToString(),
                            NumberOfSeats = Convert.ToInt32(sqlDataReader["Seats"].ToString()),
                            SeatNumbers = sqlDataReader["SeatNumbers"].ToString(),
                            Cost = Convert.ToInt32(sqlDataReader["TotalCost"]),
                            BusSource = sqlDataReader["BusSource"].ToString(),
                            BusDestination = sqlDataReader["BusDestination"].ToString(),


                        });
                    }
                    sqlConnection.Close();
                }

   
            }


            catch (SqlException sqlException)
            {
                log.Error(sqlException.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
                return ticketDetails;
        }

        public bool IfEmployeeIdExists(string CustomerEmail)
        {
            try
            {
               
                sqlConnection.Open();
                // SqlCommand sqlCommand = new SqlCommand("select * from customer where CustomerEmail='" + CustomerEmail + "'", sqlConnection);

                SqlCommand sqlCommand = new SqlCommand("CheckIfEmailExists", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@CustomerEmail", CustomerEmail);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();                
                if (sqlDataReader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an exception" + e);
                return false;
            }
            finally
            {
                sqlConnection.Close();

            }
        }





    }

}






