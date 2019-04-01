using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        IProductService productService;

        public HomeController(IProductService _productService)
        {
            productService = _productService;
        }

        public IActionResult Index()
        {
            var products = productService.GetList().Take(20).ToList();
            return View(products);
        }
    }
}
