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
        ICategoryService categoryService;

        public HomeController(IProductService _productService, ICategoryService _categoryService)
        {
            productService = _productService;
            categoryService = _categoryService;
        }

        public IActionResult Index()
        {
            var products = productService.GetList().Take(20).ToList();
            var categories = categoryService.GetList();

            List<ProductViewModel> viewModels = new List<ProductViewModel>();

            foreach (var item in products)
            {
                ProductViewModel viewModel = new ProductViewModel();
                viewModel.Id = item.Id;
                viewModel.Category = categories.SingleOrDefault(x => x.Id == item.CategoryId);
                viewModel.Description = item.Description;
                viewModel.Discounts = item.Discounts;
                viewModel.Stock = item.Stock;
                viewModel.Price = item.Price;
                viewModel.PriceWithDiscounts = item.PriceWithDiscounts;
                viewModel.Name = item.Name;
                viewModel.MerchantUserName = item.MerchantUserName;
                viewModel.ImageUrl = item.ImageUrl;

                viewModels.Add(viewModel);
            }

            return View(viewModels);
        }
    }
}
