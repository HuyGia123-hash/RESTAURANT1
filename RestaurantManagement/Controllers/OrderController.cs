using RestaurantManagement.DataAccess;
using RestaurantManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using RestaurantManagement.Models;
using System.Linq;
public class OrderController : Controller
{
 

    [HttpPost]
    public ActionResult Cuccess(List<OrderDetail> orderDetails)
    {
        int customerId = 1; 

        string query = "INSERT INTO Orders (CustomerId, OrderDate, Status) OUTPUT INSERTED.Id VALUES (@CustomerId, GETDATE(), 'Pending')";
        SqlParameter[] parameters = { new SqlParameter("@CustomerId", customerId) };
        int orderId = (int)DatabaseHelper.ExecuteQuery(query, parameters).Rows[0][0];

        foreach (var item in orderDetails)
        {
            string detailQuery = "INSERT INTO OrderDetails (OrderId, MenuItemId, Quantity, PriceAtOrder) VALUES (@OrderId, @MenuItemId, @Quantity, @Price)";
            SqlParameter[] detailParams = {
                new SqlParameter("@OrderId", orderId),
                new SqlParameter("@MenuItemId", item.MenuItemId),
                new SqlParameter("@Quantity", item.Quantity),
                new SqlParameter("@Price", item.PriceAtOrder)
            };
            DatabaseHelper.ExecuteNonQuery(detailQuery, detailParams);
        }

        return RedirectToAction("Success");
    }

    public ActionResult Success()
    {
        return View();
    }
    [HttpGet]
    public ActionResult Checkout()
    {
        Session["Cart"] = null;

        return View();
    }


    [HttpPost]
    public ActionResult CheckoutConfirm()
    {
        var cart = Session["Cart"] as List<CartItem>;
        if (cart == null || cart.Count == 0)
        {
            return RedirectToAction("Index", "Cart");
        }

        using (SqlConnection conn = new SqlConnection("Data Source=LUKEY\\MSSQLLUKEYY;Initial Catalog=QL_NhaHang;Integrated Security=True"))
        {
            conn.Open();
            SqlTransaction transaction = conn.BeginTransaction();
            try
            {
                string orderQuery = "INSERT INTO Orders (CustomerId, OrderDate, Status) OUTPUT INSERTED.Id VALUES (@CustomerId, GETDATE(), 'Pending')";
                SqlCommand orderCmd = new SqlCommand(orderQuery, conn, transaction);
                orderCmd.Parameters.AddWithValue("@CustomerId", 1); 
                int orderId = (int)orderCmd.ExecuteScalar();

                foreach (var item in cart)
                {
                    string detailQuery = "INSERT INTO OrderDetails (OrderId, MenuItemId, Quantity, PriceAtOrder) VALUES (@OrderId, @MenuItemId, @Quantity, @Price)";
                    SqlCommand detailCmd = new SqlCommand(detailQuery, conn, transaction);
                    detailCmd.Parameters.AddWithValue("@OrderId", orderId);
                    detailCmd.Parameters.AddWithValue("@MenuItemId", item.MenuItemId);
                    detailCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                    detailCmd.Parameters.AddWithValue("@Price", item.Price);
                    detailCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                Session["Cart"] = new List<CartItem>(); 
            }
            catch
            {
                transaction.Rollback();
            }
        }

        return RedirectToAction("Success");
    }
    public ActionResult VirtualCheckout()
    {
        Session["Cart"] = null;

        // Hiển thị thông báo
        TempData["SuccessMessage"] = "Thanh toán thành công! Cảm ơn bạn đã mua hàng.";

        return RedirectToAction("Index", "Menu");
    }
    public ActionResult ConfirmOrder()
    {
        var cartItems = (List<RestaurantManagement.Models.CartItem>)Session["Cart"];

        if (cartItems != null && cartItems.Any())
        {
       
            TempData["SuccessMessage"] = "Đặt món thành công!";

            Session["Cart"] = null;

            return RedirectToAction("Success");
        }
        else
        {
            TempData["ErrorMessage"] = "Giỏ hàng trống, không thể đặt món.";
            return RedirectToAction("Index", "Menu");
        }
    }

    public ActionResult Index()
    {
        return View(); // Phải tồn tại file View tương ứng
    }



}
