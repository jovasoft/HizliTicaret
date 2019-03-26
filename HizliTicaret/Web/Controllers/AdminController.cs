using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
   
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        IProductService productService;
        ICategoryService categoryService;
        IBrandService brandService;

        public AdminController(IProductService productService, ICategoryService categoryService, IBrandService brandService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.brandService = brandService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListProduct()
        {
            //var products = productService.GetList();
            //ViewData["products"] = products;
            return View();
        }

        public IActionResult AddProduct()
        {
            var getList = categoryService.GetList();
            if (getList != null)
            {
                ViewData["Categories"] = getList;
            }
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {

                Product product = new Product
                {
                   
                };
                
                productService.Add(product);
                return View("~/Views/Admin/ListCategory.cshtml");
            }

            return View(productService);
        }

        public IActionResult UpdateProduct()
        {
            return View("~/Views/Admin/AddProduct.cshtml");
        }

        public IActionResult ListCategory(string categoryTypeId)
        {
            //ViewData["Categories"] = categoryService.GetList();
            return View();
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {

                Category category = new Category {
                    Name = categoryViewModel.Name,
                    CategoryType = categoryViewModel.CategoryType
                };

                categoryService.Add(category);
                return View("~/Views/Admin/ListCategory.cshtml");
            }

            return View(categoryViewModel);
        }

        public IActionResult UpdateCategory()
        {
            return View("~/Views/Admin/AddCategory.cshtml");
        }
    }
}