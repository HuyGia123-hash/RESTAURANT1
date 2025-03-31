using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestaurantManagement.Controllers
{
    public class WaiterController : Controller
    {
        // GET: Waiter
        public ActionResult OrderList()
        {
            var cartItems = (List<RestaurantManagement.Models.CartItem>)Session["Cart"]; 
            return View(cartItems);
        }

    }
}