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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{

    public class AdminController : Controller
    {
        IProductService productService;
        ICategoryService categoryService;
        IDiscountService discountService;
        private UserManager<User> userManager;
        private RoleManager<Role> roleManager;

        private readonly IHostingEnvironment _appEnvironment;

        public AdminController(IProductService productService, ICategoryService categoryService, IDiscountService discountService, IHostingEnvironment appEnvironment, UserManager<User> _userManager, RoleManager<Role> _roleManager)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.discountService = discountService;
            this._appEnvironment = appEnvironment;
            this.userManager = _userManager;
            this.roleManager = _roleManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMerchant(string userName)
        {
            try
            {
                var user = userManager.Users.ToList().SingleOrDefault(x => x.UserName == userName);
                userManager.RemoveFromRoleAsync(user, "Merchant").Wait();
                userManager.AddToRoleAsync(user, "User").Wait();

                var products = productService.GetList().Where(x => x.MerchantUserName == userName);
                foreach (var product in products)
                {
                    productService.Delete(product.Id);
                }

            }
            catch (Exception) { }

            Response.StatusCode = 200;
            object obj = new
            {
                admins = userManager.GetUsersInRoleAsync("Admin").Result.ToList(),
                users = userManager.GetUsersInRoleAsync("User").Result.ToList(),
                merchants = userManager.GetUsersInRoleAsync("Merchant").Result.ToList()
            };

            return Json(obj);

        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddCategory()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ListRole()
        {
            ViewData["userManager"] = userManager;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddMerchant()
        {

            ViewData["Users"] = userManager.GetUsersInRoleAsync("User").Result.ToList();

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMerchant(User user)
        {
            if (!roleManager.RoleExistsAsync("Merchant").Result)
            {
                Role role = new Role { Name = "Merchant" };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            var getUser = userManager.Users.ToList().SingleOrDefault(x => x.UserName == user.UserName);
            userManager.RemoveFromRoleAsync(getUser, "User").Wait();
            userManager.AddToRoleAsync(getUser, "Merchant").Wait();

            return RedirectToAction("ListRole");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                Category mainCategory = categoryService.GetMainCategory(categoryViewModel.CategoryType);

                Category category = new Category
                {
                    Name = categoryViewModel.Name,
                    CategoryType = categoryViewModel.CategoryType,
                    MainCategoryId = mainCategory.Id,
                };

                if (categoryViewModel.File != null && categoryViewModel.File.Length > 0)
                {
                    string fileName = "assets/img/" + DateTime.Now.ToFileTime() + "_" + categoryViewModel.File.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        categoryViewModel.File.CopyTo(stream);
                    }

                    category.ImageUrl = "/" + fileName;
                }

                categoryService.Add(category);
                return RedirectToAction("ListCategory");
            }

            return View(categoryViewModel);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

                if (categoryViewModel.File != null && categoryViewModel.File.Length > 0)
                {
                    string fileName = "assets/img/" + DateTime.Now.ToFileTime() + "_" + categoryViewModel.File.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        categoryViewModel.File.CopyTo(stream);
                    }

                    category.ImageUrl = "/" + fileName;
                }

                categoryService.Update(category);

                return RedirectToAction("ListCategory");
            }

            return View(categoryViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCategory(Guid id)
        {
            if (id != Guid.Empty)
            {
                try
                {
                    var products = productService.GetList().Where(x => x.CategoryId == id);
                    foreach (var product in products)
                    {
                        productService.Delete(product.Id);
                    }
                    categoryService.Delete(id);
                }
                catch (Exception) { }
            }

            Response.StatusCode = 200;
            return Json(new { status = "success" });
        }

        #region merchant

        [Authorize(Roles = "Admin, Merchant")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Merchant")]
        public IActionResult ListCategory(string categoryTypeId)
        {
            List<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();
            List<Category> categories = new List<Category>();
            List<Product> products = new List<Product>();
            categories = categoryService.GetList();

            if (categories != null)
            {
                foreach (var category in categories)
                {
                    if (category.MainCategoryId != Guid.Empty)
                    {
                        CategoryViewModel model = new CategoryViewModel();
                        products = productService.GetList().Where(x => x.CategoryId == category.Id).ToList();
                        model.Name = category.Name;
                        model.CategoryType = category.CategoryType;
                        model.Id = category.Id;
                        model.ProductCount = products.Count();
                        categoryViewModels.Add(model);
                    }

                }
            }

            return View(categoryViewModels);
        }

        [Authorize(Roles = "Admin, Merchant")]
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
                    IsAvailable = productViewModel.IsAvailable,
                    MerchantUserName = User.Identity.Name,
                    Description = productViewModel.Description
                };

                if (productViewModel.File != null && productViewModel.File.Length > 0)
                {
                    string fileName = "assets/img/" + DateTime.Now.ToFileTime() + "_" + productViewModel.File.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        productViewModel.File.CopyTo(stream);
                    }

                    product.ImageUrl = "/" + fileName;
                }

                productService.Add(product);
                return RedirectToAction("ListProduct");
            }

            return View(productService);
        }

        [Authorize(Roles = "Admin, Merchant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDiscount(DiscountViewModel discountViewModel)
        {
            Category mainCategory = categoryService.Get(discountViewModel.MainCategoryId);
            Category category = categoryService.Get(discountViewModel.CategoryId);

            var products = new List<Product>();

            //tüm kategorilerse
            if (category == null && mainCategory == null)
            {
                if (User.IsInRole("Merchant"))
                    products = productService.GetList().Where(x => x.MerchantUserName == User.Identity.Name).ToList();
                else
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

                    if (User.IsInRole("Merchant"))
                        allProducts = productService.GetList().Where(x => x.MerchantUserName == User.Identity.Name).ToList();

                    //tüm kategorilere ait ürünleri al
                    foreach (var product in allProducts)
                    {
                        if (categoyProducts.Exists(x => x.Id == product.CategoryId))
                            products.Add(product);
                    }
                }
                else if (discountViewModel.ProductId == Guid.Empty) //kategoriyse
                {
                    if (User.IsInRole("Merchant"))
                        products = productService.GetList().Where(x => x.MerchantUserName == User.Identity.Name && x.CategoryId == discountViewModel.CategoryId).ToList();
                    else
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

        [Authorize(Roles = "Admin, Merchant")]
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

        [Authorize(Roles = "Admin, Merchant")]
        public IActionResult AddDiscount()
        {
            var getList = categoryService.GetList();

            if (getList != null)
            {
                ViewData["Categories"] = getList;
            }

            return View();
        }

        [Authorize(Roles = "Admin, Merchant")]
        public IActionResult ListProduct()
        {
            List<ProductViewModel> productViewModels = new List<ProductViewModel>();
            List<Product> Urunler = new List<Product>();
            Urunler = productService.GetList();

            if (Urunler != null)
            {
                if (User.IsInRole("Merchant"))
                    Urunler = productService.GetList().Where(x => x.MerchantUserName == User.Identity.Name).ToList();

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

        [Authorize(Roles = "Admin, Merchant")]
        public IActionResult ListDiscount()
        {
            //List<ProductViewModel> productViewModels = new List<ProductViewModel>();
            //List<Product> Urunler = new List<Product>();
            //Urunler = productService.GetList();

            //if (Urunler != null)
            //{
            //   if (User.IsInRole("Merchant"))
            //      Urunler = productService.GetList().Where(x => x.MerchantUserName == User.Identity.Name).ToList();

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

        [Authorize(Roles = "Admin, Merchant")]
        public IActionResult AddProduct()
        {
            var getList = categoryService.GetList();

            if (getList != null)
            {
                ViewData["Categories"] = getList;
            }

            return View();
        }

        [Authorize(Roles = "Admin, Merchant")]
        public IActionResult ListProductFilter(int productId)
        {
            List<ProductViewModel> productViewModels = new List<ProductViewModel>();

            List<Product> filterProducts = new List<Product>();

            if (filterProducts != null)
            {
                if (productId == 0)
                {
                    productViewModels = new List<ProductViewModel>();

                    if (User.IsInRole("Merchant"))
                        filterProducts = productService.GetList().Where(x => x.MerchantUserName == User.Identity.Name && x.IsAvailable == true).ToList();
                    else
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
                    if (User.IsInRole("Merchant"))
                        filterProducts = productService.GetList().Where(x => x.MerchantUserName == User.Identity.Name && x.IsAvailable == false).ToList();
                    else
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
                    if (User.IsInRole("Merchant"))
                        filterProducts = productService.GetList().Where(x => x.MerchantUserName == User.Identity.Name).ToList();
                    else
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

        [Authorize(Roles = "Admin, Merchant")]
        public IActionResult ListCategoryFilter(int categoryId)
        {

            List<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();
            List<Category> categories = new List<Category>();
            List<Product> products = new List<Product>();
            categories = categoryService.GetList();

            if (categories != null)
            {
                if(Convert.ToInt32(categoryId) == 3)
                {
                    categories = categoryService.GetList().Where(x => x.MainCategoryId != Guid.Empty).ToList();

                    foreach (var category in categories)
                    {
                       
                        CategoryViewModel model = new CategoryViewModel();
                        products = productService.GetList().Where(x => x.CategoryId == category.Id).ToList();
                        model.Name = category.Name;
                        model.CategoryType = category.CategoryType;
                        model.Id = category.Id;
                        model.ProductCount = products.Count();
                        model.MainCategoryId = category.MainCategoryId;
                        categoryViewModels.Add(model);
                    }
                }
                else
                {
                    categories = categoryService.GetList().Where(x => x.MainCategoryId != Guid.Empty && x.CategoryType == (CategoryTypes)categoryId).ToList();
                    foreach (var category in categories)
                    {
                        CategoryViewModel model = new CategoryViewModel();
                        products = productService.GetList().Where(x => x.CategoryId == category.Id).ToList();
                        model.Name = category.Name;
                        model.CategoryType = category.CategoryType;
                        model.Id = category.Id;
                        model.ProductCount = products.Count();
                        model.MainCategoryId = category.MainCategoryId;
                        categoryViewModels.Add(model);
                    }
   
                }
                
            }

            return Json(categoryViewModels);
        }

        [Authorize(Roles = "Admin, Merchant")]
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

        [Authorize(Roles = "Admin, Merchant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProduct(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                Product product = productService.Get(productViewModel.Id);
                product.Name = productViewModel.Name;
                product.Price = productViewModel.Price;
                product.Stock = productViewModel.Stock;
                product.IsAvailable = productViewModel.IsAvailable;
                product.CategoryId = productViewModel.Category.Id;
                product.Description = productViewModel.Description;

                if (productViewModel.File != null || productViewModel.File.Length > 0)
                {
                    string fileName = "assets/img/" + DateTime.Now.ToFileTime() + "_" + productViewModel.File.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        productViewModel.File.CopyTo(stream);
                    }

                    product.ImageUrl = "/" + fileName;
                }

                productService.Update(product);

                return RedirectToAction("ListProduct");
            }

            return View(productViewModel);
        }

        [Authorize(Roles = "Admin, Merchant")]
        public IActionResult AddProductAltKategorileriGetir(Guid KategoriID)
        {

            List<Category> TumAltKategoriler = new List<Category>();

            TumAltKategoriler = categoryService.GetList();

            List<Category> altkategoriler = TumAltKategoriler.Where(k => k.MainCategoryId == KategoriID).ToList();
            return Json(altkategoriler);
        }

        [Authorize(Roles = "Admin, Merchant")]
        public IActionResult AddDiscountAltUrunleriGetir(Guid KategoriID)
        {

            List<Product> TumAltUrunler = new List<Product>();

            if (User.IsInRole("Merchant"))
                TumAltUrunler = productService.GetList().Where(x => x.MerchantUserName == User.Identity.Name).ToList();
            else
                TumAltUrunler = productService.GetList();

            List<Product> altkategoriler = TumAltUrunler.Where(k => k.CategoryId == KategoriID).ToList();
            return Json(altkategoriler);
        }

        [Authorize(Roles = "Admin, Merchant")]
        public IActionResult ListRoleFilter(int ListId)
        {
            if (ListId == 0)
            {
                return Json(userManager.GetUsersInRoleAsync("User").Result.ToList());
            }
            else if (ListId == 1)
            {
                return Json(userManager.GetUsersInRoleAsync("Merchant").Result.ToList());
            }
            else if (ListId == 2)
            {
                return Json(userManager.GetUsersInRoleAsync("Admin").Result.ToList());
            }
            else if (ListId == 3)
            {
                object obj = new
                {
                    admins = userManager.GetUsersInRoleAsync("Admin").Result.ToList(),
                    users = userManager.GetUsersInRoleAsync("User").Result.ToList(),
                    merchants = userManager.GetUsersInRoleAsync("Merchant").Result.ToList()
                };

                return Json(obj);
            }

            return View();
        }
        #endregion
    }

}