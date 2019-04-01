using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Helpers;
using Web.Models;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                ViewData["Cart"] = new List<CartItem>();
                ViewData["CartTotal"] = 0;
                ViewData["CartCount"] = 0;
            }
            else
            {
                ViewData["Cart"] = cart;
                ViewData["CartTotal"] = cart.Sum(item => (item.Product.Price - (item.Product.Price * item.Product.Discounts) / 100) * item.Quantity);
                ViewData["CartCount"] = cart.Count;
            }
            return View();
        }
    }
}