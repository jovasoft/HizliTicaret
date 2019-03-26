using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class CategoryController : Controller
    {
        IProductService productService;
        ICategoryService categoryService;

        public CategoryController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }

        public IActionResult Index(Guid id)
        {
            Category category = categoryService.Get(id);

            if (category == null)
            {
                return Redirect("/");
            }

            var products = productService.GetProductsInCategory(id);

            CategoryIndexViewModel categoryIndexViewModel = new CategoryIndexViewModel { Category = category, Products = products };

            return View(categoryIndexViewModel);
        }
    }
}