using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        IDiscountService discountService;

        private readonly IHostingEnvironment _appEnvironment;

        public AdminController(IProductService productService, ICategoryService categoryService, IBrandService brandService, IDiscountService discountService, IHostingEnvironment appEnvironment)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.brandService = brandService;
            this.discountService = discountService;
            this._appEnvironment = appEnvironment;
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
                    model.MainCategory = categoryService.GetMainCategory(model.Category.CategoryType).Name;
                    model.Price = urun.Price;
                    model.Stock = urun.Stock;
                    model.Id = urun.Id;

                    productViewModels.Add(model);
                }
            }

            return View(productViewModels);
        }

        public IActionResult ListDiscount()
        {
            //List<ProductViewModel> productViewModels = new List<ProductViewModel>();
            //List<Product> Urunler = new List<Product>();
            //Urunler = productService.GetList();

            //if (Urunler != null)
            //{
            //    foreach (var urun in Urunler)
            //    {
            //        ProductViewModel model = new ProductViewModel();
            //        model.Name = urun.Name;
            //        model.Category = categoryService.Get(urun.CategoryId);
            //        model.MainCategory = categoryService.GetMainCategory(model.Category.CategoryType).Name;
            //        model.Price = urun.Price;
            //        model.Stock = urun.Stock;
            //        model.Id = urun.Id;

            //        productViewModels.Add(model);
            //    }
            //}

            //return View(productViewModels);
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
                if (productViewModel.File == null || productViewModel.File.Length == 0) return Content("Lütfen ürün resmi seçin.");
                if (productViewModel.Category == null) return Content("Lütfen geri dönün ve kategori seçin.");

                string fileName = "assets/img/" + DateTime.Now.ToFileTime() + "_" + productViewModel.File.FileName;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    productViewModel.File.CopyTo(stream);
                }

                Product product = new Product
                {
                    Name = productViewModel.Name,
                    Price = productViewModel.Price,
                    Stock = productViewModel.Stock,
                    CategoryId = productViewModel.Category.Id,
                    IsAvailable = productViewModel.IsAvailable,
                    MerchantUserName = User.Identity.Name,
                    Description = productViewModel.Description,
                    ImageUrl = "/" + fileName
                };
                
                productService.Add(product);
                return RedirectToAction("ListProduct");
            }

            return View(productService);
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
                        model.Id = kategori.Id;
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
                return RedirectToAction("ListCategory");
            }

            return View(categoryViewModel);
        }

        public IActionResult UpdateCategory(Guid id)
        {
            if (id != Guid.Empty)
            {
                Category Kategoriler;

                Kategoriler = categoryService.Get(id);

                CategoryViewModel model = new CategoryViewModel();
                model.Name = Kategoriler.Name;
                model.CategoryType = Kategoriler.CategoryType;
                model.Id = Kategoriler.Id;

                return View(model);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCategory(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                Category category = categoryService.Get(categoryViewModel.Id);
                Category mainCategory = categoryService.GetMainCategory(categoryViewModel.CategoryType);


                category.Name = categoryViewModel.Name;
                category.CategoryType = categoryViewModel.CategoryType;
                category.MainCategoryId = mainCategory.Id;

                categoryService.Update(category);

                return RedirectToAction("ListCategory");
            }

            return View(categoryViewModel);
        }

        public IActionResult UpdateProduct(Guid id)
        {
            if (id != Guid.Empty)
            {
                Product product;

                product = productService.Get(id);

                ProductViewModel model = new ProductViewModel();
                model.Name = product.Name;
                model.Id = product.Id;
                model.Category = categoryService.Get(product.CategoryId);
                model.MainCategory = categoryService.GetMainCategory(model.Category.CategoryType).Name;
                model.Price = product.Price;
                model.Stock = product.Stock;
                model.Description = product.Description;

                var getList = categoryService.GetList();

                if (getList != null)
                {
                    ViewData["Categories"] = getList;
                }

                return View(model);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProduct(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                if (productViewModel.File == null || productViewModel.File.Length == 0) return Content("Lütfen ürün resmi seçin.");
                if (productViewModel.Category == null) return Content("Lütfen geri dönün ve kategori seçin.");

                string fileName = "assets/img/" + DateTime.Now.ToFileTime() + "_" + productViewModel.File.FileName;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    productViewModel.File.CopyTo(stream);
                }

                Product product = productService.Get(productViewModel.Id);
                product.Name = productViewModel.Name;
                product.Price = productViewModel.Price;
                product.Stock = productViewModel.Stock;
                product.IsAvailable = productViewModel.IsAvailable;
                product.CategoryId = productViewModel.Category.Id;
                product.Description = productViewModel.Description;
                product.ImageUrl = "/" + fileName;

                productService.Update(product);

                return RedirectToAction("ListProduct");
            }

            return View(productViewModel);
        }

        public IActionResult AddProductAltKategorileriGetir(Guid KategoriID)
        {

            List<Category> TumAltKategoriler = new List<Category>();

            TumAltKategoriler = categoryService.GetList();

            List<Category> altkategoriler = TumAltKategoriler.Where(k => k.MainCategoryId == KategoriID).ToList();
            return Json(altkategoriler);
        }

        public IActionResult AddDiscountAltUrunleriGetir(Guid KategoriID)
        {

            List<Product> TumAltUrunler = new List<Product>();

            TumAltUrunler = productService.GetList();

            List<Product> altkategoriler = TumAltUrunler.Where(k => k.CategoryId == KategoriID).ToList();
            return Json(altkategoriler);
        }

        public IActionResult ListCategoryFilter(int categoryId)
        {
            List<Category> categories = new List<Category>();

            categories = categoryService.GetList();

            List<Category> filterCategories;

            if (Convert.ToInt32(categoryId) == 3)
            {
               filterCategories = categories.Where(k => k.MainCategoryId != Guid.Empty).ToList();
            }
            else
            {
                filterCategories = categories.Where(k => k.CategoryType == (CategoryTypes)categoryId && k.MainCategoryId != Guid.Empty).ToList();
            }

            return Json(filterCategories);
        }

        public IActionResult ListProductFilter(int productId)
        {
            List<ProductViewModel> productViewModels = new List<ProductViewModel>();

            List<Product> filterProducts = new List<Product>();

            if (filterProducts != null)
            {
                if (productId == 0)
                {
                    productViewModels = new List<ProductViewModel>();
                    filterProducts = productService.GetList().Where(k => k.IsAvailable == true).ToList();

                    foreach (var product in filterProducts)
                    {
                        ProductViewModel model = new ProductViewModel();
                        model.Name = product.Name;
                        model.Category = categoryService.Get(product.CategoryId);
                        model.MainCategory = categoryService.Get(model.Category.MainCategoryId).Name;
                        model.Price = product.Price;
                        model.Stock = product.Stock;
                        model.IsAvailable = product.IsAvailable;
                        model.Id = product.Id;

                        productViewModels.Add(model);

                    }

                }
                else if (productId == 1)
                {
                    productViewModels = new List<ProductViewModel>();
                    filterProducts = productService.GetList().Where(k => k.IsAvailable == false).ToList();

                    foreach (var product in filterProducts)
                    {
                        ProductViewModel model = new ProductViewModel();
                        model.Name = product.Name;
                        model.Category = categoryService.Get(product.CategoryId);
                        model.MainCategory = categoryService.Get(model.Category.MainCategoryId).Name;
                        model.Price = product.Price;
                        model.Stock = product.Stock;
                        model.IsAvailable = product.IsAvailable;
                        model.Id = product.Id;

                        productViewModels.Add(model);

                    }

                }
                else if (productId == 2)
                {
                    productViewModels = new List<ProductViewModel>();
                    filterProducts = productService.GetList().ToList();

                    foreach (var product in filterProducts)
                    {
                        ProductViewModel model = new ProductViewModel();
                        model.Name = product.Name;
                        model.Category = categoryService.Get(product.CategoryId);
                        model.MainCategory = categoryService.Get(model.Category.MainCategoryId).Name;
                        model.Price = product.Price;
                        model.Stock = product.Stock;
                        model.IsAvailable = product.IsAvailable;
                        model.Id = product.Id;

                        productViewModels.Add(model);

                    }

                }
            }

            return Json(productViewModels);
        }

        public IActionResult DeleteCategory(Guid id)
        {
            if (id != Guid.Empty)
            {
                try
                {
                    categoryService.Delete(id);
                }
                catch (Exception) { }
            }

            Response.StatusCode = 200;
            return Json(new { status = "success" });
        }

        public IActionResult DeleteProduct(Guid id)
        {
            if (id != Guid.Empty)
            {
                try
                {
                    productService.Delete(id);
                }
                catch (Exception) { }
            }

            Response.StatusCode = 200;
            return Json(new { status = "success" });
        }

        public IActionResult AddDiscount()
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
        public IActionResult AddDiscount(DiscountViewModel discountViewModel)
        {
            Category mainCategory = categoryService.Get(discountViewModel.MainCategoryId);
            Category category = categoryService.Get(discountViewModel.CategoryId);

            var products = new List<Product>();

            //ana kategoriyse
            if (category == null && mainCategory == null)
            {
                products = productService.GetList();
            }
            else
            {
                //ana kategoriyse
                if (category.Id == null)
                {
                    var allCategories = categoryService.GetList();

                    //ana kategorinin tüm kategorilerini al
                    var categoyProducts = new List<Category>();
                    foreach (var cat in allCategories)
                    {
                        if (cat.MainCategoryId == discountViewModel.CategoryId)
                            categoyProducts.Add(cat);
                    }

                    var allProducts = productService.GetList();

                    //tüm kategorilere ait ürünleri al
                    foreach (var product in allProducts)
                    {
                        if (categoyProducts.Exists(x => x.Id == product.CategoryId))
                            products.Add(product);
                    }
                }
                else if(discountViewModel.ProductId == Guid.Empty) //kategoriyse
                {
                    products = productService.GetProductsInCategory(discountViewModel.CategoryId);
                }
                else
                {
                    //product
                    products.Add(productService.Get(discountViewModel.ProductId));
                }
            }

            foreach (var product in products)
            {
                product.Discounts += discountViewModel.Percent;

                productService.Update(product);

                Discount discount = new Discount
                {
                    ProductId = product.Id,
                    Percent = discountViewModel.Percent,
                    MerchantUserName = User.Identity.Name
                };

                discountService.Add(discount);
            }

            return RedirectToAction("ListDiscount");
        }

    }

}