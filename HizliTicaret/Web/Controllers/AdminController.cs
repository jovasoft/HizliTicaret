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
            List<ProductViewModel> productViewModels = new List<ProductViewModel>();
            List<Product> Urunler = new List<Product>();
            Urunler = productService.GetList();

            if (Urunler != null)
            {
                foreach (var urun in Urunler)
                {
                    ProductViewModel model = new ProductViewModel();
                    model.Name = urun.Name;
                    model.Category = categoryService.Get(urun.CategoryId);
                    model.MainCategory = categoryService.Get(model.Category.MainCategoryId).Name;
                    model.Price = urun.Price;
                    model.Stock = urun.Stock;

                    productViewModels.Add(model);
                }
            }

            return View(productViewModels);
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
                    Name = productViewModel.Name,
                    Price = productViewModel.Price,
                    Stock = productViewModel.Stock,
                    CategoryId = productViewModel.Category.Id,
                    IsAvailable = true
                   
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
            List<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();
            List<Category> Kategoriler = new List<Category>();
            Kategoriler = categoryService.GetList();
            
            if (Kategoriler != null)
            {
                foreach (var kategori in Kategoriler)
                {
                    if (kategori.MainCategoryId != Guid.Empty )
                    {
                        CategoryViewModel model = new CategoryViewModel();
                        model.Name = kategori.Name;
                        model.CategoryType = kategori.CategoryType;
                        categoryViewModels.Add(model);
                    }
                   
                }
            }

            return View(categoryViewModels);
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
                Category mainCategory = categoryService.GetMainCategory(categoryViewModel.CategoryType);

                Category category = new Category {
                    Name = categoryViewModel.Name,
                    CategoryType = categoryViewModel.CategoryType,
                    MainCategoryId = mainCategory.Id
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

        public IActionResult AltKategorileriGetir(Guid KategoriID)
        {

            List<Category> TumAltKategoriler = new List<Category>();

            TumAltKategoriler = categoryService.GetList();

            List<Category> altkategoriler = TumAltKategoriler.Where(k => k.MainCategoryId == KategoriID).ToList();
            return Json(altkategoriler);
        }
    }

}