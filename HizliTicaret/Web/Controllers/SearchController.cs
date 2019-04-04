using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class SearchController : Controller
    {
        IProductService productService;

        public SearchController(IProductService _productService)
        {
            productService = _productService;
        }

        public IActionResult Index(string query)
        {
            var products = productService.GetList().Where(x => x.Name.ToLower().Contains(query.ToLower())).ToList();

            ViewData["query"] = query;
            return View(products);
        }
    }
}