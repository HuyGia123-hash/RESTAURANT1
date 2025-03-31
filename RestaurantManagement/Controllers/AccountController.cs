using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.SqlClient;
using RestaurantManagement.Models;

namespace RestaurantManagement.Controllers
{
    public class AccountController : Controller
    {
        private string connectionString = "Data Source=LUKEY\\MSSQLLUKEYY;Initial Catalog=QL_NhaHang;Integrated Security=True";

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, FullName, RoleId FROM Users WHERE Email = @Email AND PasswordHash = HASHBYTES('SHA2_256', @Password)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int userId = reader.GetInt32(0);
                    string fullName = reader.GetString(1);
                    int roleId = reader.GetInt32(2);

                    Session["UserId"] = userId;
                    Session["FullName"] = fullName;
                    Session["RoleId"] = roleId;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Email hoặc mật khẩu không đúng!";
                    return View();
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
