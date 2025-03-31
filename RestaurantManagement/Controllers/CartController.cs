using RestaurantManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
public class CartController : Controller
{
    private const string CartSessionKey = "Cart";

    private List<CartItem> GetCart()
    {
        var cart = Session[CartSessionKey] as List<CartItem>;
        if (cart == null)
        {
            cart = new List<CartItem>();
            Session[CartSessionKey] = cart;
        }
        return cart;
    }

    public ActionResult AddToCart(int id, string name, decimal price)
    {
        var cart = GetCart();
        var existingItem = cart.FirstOrDefault(x => x.MenuItemId == id);

        if (existingItem != null)
        {
            existingItem.Quantity++; 
        }
        else
        {
            cart.Add(new CartItem { MenuItemId = id, Name = name, Price = price, Quantity = 1 });
        }

        return Redirect(Request.UrlReferrer.ToString());
    }


    public ActionResult Index()
    {
        var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
        ViewBag.CartCount = cart.Sum(x => x.Quantity);
        return View(cart);
    }

    public ActionResult Remove(int id)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(x => x.MenuItemId == id);
        if (item != null)
        {
            cart.Remove(item);
        }
        return RedirectToAction("Index");
    }

    public ActionResult Clear()
    {
        Session[CartSessionKey] = new List<CartItem>();
        return RedirectToAction("Index");
    }
}
