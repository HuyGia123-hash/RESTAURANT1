using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using RestaurantManagement.Models;

namespace RestaurantManagement.Controllers
{
    public class MenuController : Controller
    {
        private static readonly string connectionString;

        static MenuController()
        { 
            connectionString = ConfigurationManager.ConnectionStrings["RestaurantDB"]?.ConnectionString;
          }
        

        public ActionResult Index()
        {
            List<MenuItem> menuItems = new List<MenuItem>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT Id, Name, Price, Description, Category, IsAvailable 
                                     FROM MenuItems 
                                     ORDER BY Category, Name";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                menuItems.Add(new MenuItem
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Price = reader.GetDecimal(2),
                                    Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                    Category = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                    IsAvailable = reader.GetBoolean(5)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error while retrieving menu items: " + ex.Message;
                return View("Error");
            }

            return View(menuItems);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(MenuItem menuItem)
        {
            if (!ModelState.IsValid)
                return View(menuItem);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO MenuItems (Name, Price, Description, Category, IsAvailable) VALUES (@Name, @Price, @Description, @Category, @IsAvailable)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", menuItem.Name);
                cmd.Parameters.AddWithValue("@Price", menuItem.Price);
                cmd.Parameters.AddWithValue("@Description", (object)menuItem.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Category", (object)menuItem.Category ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsAvailable", menuItem.IsAvailable);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}