using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RestaurantManagement.Models;

public class TableDAL
{
    private static readonly string connectionString = "Data Source=LUKEY\\MSSQLLUKEYY;Initial Catalog=QL_NhaHang;Integrated Security=True";

    public List<Table> GetAvailableTables()
    {
        List<Table> tables = new List<Table>();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT Id, TableName, Capacity, IsAvailable FROM Tables WHERE IsAvailable = 1";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (
                    SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(new Table
                        {
                            Id = reader.GetInt32(0),
                            TableName = reader.GetString(1),
                            Capacity = reader.GetInt32(2),
                            IsAvailable = reader.GetBoolean(3)
                        });
                    }
                }
            }
        }
        return tables;
    }
}
