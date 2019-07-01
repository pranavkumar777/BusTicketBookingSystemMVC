

using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class CustomerDAL
    {
        string con = @"server=ASPIRE1251; database=bus; integrated Security=false; user=sa; password=aspire@123";
        SqlConnection sqlConnection = null;
        //  SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);


        public bool AddNewCustomer(CustomerModel customer)
        {
            try
            {
                sqlConnection = new SqlConnection(con);
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
            catch(SqlException)
            {
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;


        }

        public bool LoginValidate(CustomerModel customer)
        {

            sqlConnection = new SqlConnection(con);
            sqlConnection.Open();
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand("Select * from Customer where CustomerEmail=@CustomerEmail and CustomerPassword=@CustomerPassword", sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@CustomerEmail", customer.CustomerEmail);
                    sqlCommand.Parameters.AddWithValue("@CustomerPassword", customer.CustomerPassword);
                    //  SqlCommand sqlCommand = new SqlCommand("Select * from Customer where CustomerEmail=@email and CustomerPassword=@password", sqlConnection);
                    DataTable dataTable = new DataTable();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataTable);
                    if (dataTable.Rows.Count > 0)
                    {
                        return true;
                    }
                    return false;
                    /*     int usercount = (Int32)sqlCommand.ExecuteScalar();
                         if (usercount == 1)
                             return true;
                         return false; */

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


        public List<BusModel> DisplayBusDetails(BusModel bus)
        {
            List<BusModel> BusDetails = new List<BusModel>();
            try
            {
                sqlConnection = new SqlConnection(con);
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("Select * from BusDetails where BusSource=@source and BusDestination=@destination and BusDepartureDate=@date", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@source", bus.BusSource);
                sqlCommand.Parameters.AddWithValue("@destination", bus.BusDestination);
                sqlCommand.Parameters.AddWithValue("@date", bus.BusDepartureDate);

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
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
            catch (SqlException)
            {
            }

            finally { sqlConnection.Close(); }

            return BusDetails;
        }
        
               public List<BusModel> DisplaySeats(int BusID)
                {
                    List<BusModel> seatList = new List<BusModel>();
                    sqlConnection = new SqlConnection(con);
                    sqlConnection.Open();

                    try
                    {


                        using (SqlCommand sqlCommand = new SqlCommand("Select * from  SeatAllocation where BusID=@BusID ", sqlConnection))
                        {
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
                            using (SqlCommand sqlCommand = new SqlCommand("Select * from BusDetails where BusID=@BusId ", sqlConnection))
                            {
                                sqlConnection.Open();
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
                    catch (SqlException)
                    {
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                    return seatList;
                }

        /*          public List<TicketModel> Payment(TicketModel ticket)
                  {

                      List<TicketModel> ticketDetails = new List<TicketModel>();
                      ticketDetails.Add(new TicketModel
                      {
                          BusID = ticket.BusID,
                          seats = ticket.seats
                      }
                          );
                      return ticketDetails;
                  }

              } 
             */
        public List<TicketModel> Payment(TicketModel ticket)
        {

            List<TicketModel> ticketDetails = new List<TicketModel>();
            sqlConnection = new SqlConnection(con);
            try
            {
                sqlConnection.Open();
                int seatCount = ticket.seats.Length;
                int totalCost = seatCount * ticket.Cost;
                string query = "INSERT INTO SeatAllocation (BusID,BusSource,BusDestination,SeatNumber,BusTicketCost) Values(@BusID,@Bussource,@Busdestination,@SeatNumber,@Cost)";

                using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO PaymentDetails(BusID,BusSource,BusDestination,CustomerEmail,Seats,SeatNumbers,TotalCost)values(@Busid,@BusSource,@BusDestination,@Email,@seat,@seatnumbers,@totalcost)", sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Busid", ticket.BusID);
                    sqlCommand.Parameters.AddWithValue("@Email", ticket.CustomerEmail);
                    sqlCommand.Parameters.AddWithValue("@seat", seatCount);
                    sqlCommand.Parameters.AddWithValue("@totalcost", totalCost);
                    sqlCommand.Parameters.AddWithValue("@BusSource", ticket.BusSource);
                    sqlCommand.Parameters.AddWithValue("@BusDestination", ticket.BusDestination);
                    sqlCommand.Parameters.AddWithValue("@seatnumbers", ticket.SeatNumbers);
                    sqlCommand.ExecuteNonQuery();
                }
                for (int i = 0; i < seatCount; i++)
                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
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
                  //  seats=ticket.seats,
                    Cost = ticket.Cost,
                    BusSource = ticket.BusSource,
                    BusDestination = ticket.BusDestination
                });
            }
            catch (SqlException)
            {

            }
            finally
            {
                sqlConnection.Close();
            }

            return ticketDetails;
        }
       

        public List<TicketModel> BookingDetails(string CustomerEmail)
        {
            List<TicketModel> ticketDetails = new List<TicketModel>();
            sqlConnection = new SqlConnection(con);
            sqlConnection.Open();
            
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand("Select * from PaymentDetails where CustomerEmail=@email ", sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@email", CustomerEmail);
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                   
                    while (sqlDataReader.Read())
                    {
                        ticketDetails.Add(new TicketModel
                        {
                            BusID = Convert.ToInt32(sqlDataReader["BusID"]),
                            CustomerEmail = sqlDataReader["CustomerEmail"].ToString(),
                            NumberOfSeats = Convert.ToInt32(sqlDataReader["Seats"].ToString()),
                            SeatNumbers=sqlDataReader["SeatNumbers"].ToString(),
                            Cost = Convert.ToInt32(sqlDataReader["TotalCost"]),
                            BusSource = sqlDataReader["BusSource"].ToString(),
                            BusDestination = sqlDataReader["BusDestination"].ToString()
                        });
                    }
                    sqlConnection.Close();
                }
               
               
            }
            catch (SqlException)
            {
            }
            finally
            {
                sqlConnection.Close();
            }
                return ticketDetails;
        }
}

}






