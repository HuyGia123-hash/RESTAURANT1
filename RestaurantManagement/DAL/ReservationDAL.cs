using RestaurantManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace RestaurantManagement.DAL
{
    public class ReservationDAL
    {
        private static readonly string connectionString = "Data Source=LUKEY\\MSSQLLUKEYY;Initial Catalog=QL_NhaHang;Integrated Security=True";

        public bool AddReservation(Reservation reservation)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string getCountQuery = "SELECT COUNT(*) FROM Reservations";
                int count = 0;

                using (SqlCommand cmd = new SqlCommand(getCountQuery, con))
                {
                    con.Open();
                    count = (int)cmd.ExecuteScalar();
                    con.Close();
                }

                string customerCode = "KH" + (count + 1).ToString("D3");

                string query = @"INSERT INTO Reservations (CustomerCode, CustomerName, PhoneNumber, ReservationDate, ArrivalDate, NumberOfGuests, Status) 
                         VALUES (@CustomerCode, @CustomerName, @PhoneNumber, @ReservationDate, @ArrivalDate, @NumberOfGuests, N'Chờ xác nhận')";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CustomerCode", customerCode);
                    cmd.Parameters.AddWithValue("@CustomerName", reservation.CustomerName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", reservation.PhoneNumber);
                    cmd.Parameters.AddWithValue("@ReservationDate", reservation.ReservationDate);
                    cmd.Parameters.AddWithValue("@ArrivalDate", reservation.ArrivalDate);
                    cmd.Parameters.AddWithValue("@NumberOfGuests", reservation.NumberOfGuests);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Reservation> GetPendingReservations()
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT Id, CustomerName, PhoneNumber, ReservationDate, ArrivalDate, NumberOfGuests, Status 
                         FROM Reservations 
                         WHERE Status = N'Chờ xác nhận'";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservations.Add(new Reservation
                            {
                                Id = reader.GetInt32(0),
                                CustomerName = reader.GetString(1),
                                PhoneNumber = reader.GetString(2),
                                ReservationDate = reader.GetDateTime(3),
                                ArrivalDate = reader.GetDateTime(4),
                                NumberOfGuests = reader.GetInt32(5),
                            });
                        }
                    }
                }
            }
            return reservations;
        }
        public bool ConfirmReservation(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Reservations SET Status = N'Đã xác nhận' WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

    }
}
