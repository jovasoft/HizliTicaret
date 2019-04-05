using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        IProductService productService;
        ICategoryService categoryService;
        IVisitService visitService;
        IPopupService popupService;

        public HomeController(IProductService _productService, ICategoryService _categoryService, IVisitService _visitService, IPopupService _popupService)
        {
            productService = _productService;
            categoryService = _categoryService;
            visitService = _visitService;
            popupService = _popupService;
        }

        public IActionResult Index()
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            if (!visitService.IsVisitedToday(ipAddress))
            {
                Visit visit = new Visit();
                visit.IPAddress = ipAddress;
                visit.Date = DateTime.Now;

                visitService.Add(visit);
            }

            var getPopups = popupService.GetList();
            foreach (var popup in getPopups)
            {
                if (popup.Status == true)
                {
                    ViewData["PopupMessage"] = popup.Message;
                    ViewData["PopupStatus"] = true;
                }
            }

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
