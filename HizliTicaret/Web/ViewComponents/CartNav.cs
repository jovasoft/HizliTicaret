using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Helpers;
using Web.Models;

namespace Web.ViewComponents
{
    public class CartNav : ViewComponent
    {
        public IViewComponentResult Invoke()
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
                ViewData["CartTotal"] = cart.Sum(item => item.Product.Price * item.Quantity);
                ViewData["CartCount"] = cart.Count;
            }

            return View();
        }
    }
}
