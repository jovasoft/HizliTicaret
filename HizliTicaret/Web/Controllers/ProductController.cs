using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Helpers;
using Web.Models;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        IFavoriteService favoriteService;
        IProductService productService;
        ICategoryService categoryService;
        ISaleService saleService;

        public ProductController(IFavoriteService favoriteService, IProductService productService, ICategoryService categoryService, ISaleService saleService)
        {
            this.favoriteService = favoriteService;
            this.productService = productService;
            this.categoryService = categoryService;
            this.saleService = saleService;
        }

        public IActionResult Show(Guid id)
        {
            if (id != Guid.Empty)
            {
                Product product = productService.Get(id);
                Category category = categoryService.Get(product.CategoryId);
                Category mainCat = categoryService.GetMainCategory(CategoryTypes.Women);
                bool isFavorited = favoriteService.GetList(User.Identity.Name).Exists(x => x.ProductId == product.Id);

                ViewData["ProductCategory"] = category;
                ViewData["ProductMainCategoryName"] = mainCat.Name;
                ViewData["IsFavorited"] = isFavorited;

                return View(product);
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User, Merchant")]
        public IActionResult AddFavorite(Guid id)
        {
            if (id != Guid.Empty)
            {
                bool isFavorited = favoriteService.GetList(User.Identity.Name).Exists(x => x.ProductId == id);

                if (!isFavorited)
                {
                    Favorite favorite = new Favorite();
                    favorite.ProductId = id;
                    favorite.UserName = User.Identity.Name;
                    favorite.ProductName = productService.Get(id).Name;
                    favorite.CreatedDate = DateTime.Now.ToString();

                    favoriteService.Add(favorite);
                }
            }

            Response.StatusCode = 200;
            return Json(new { status = "success" });
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User, Merchant")]
        public IActionResult Buy(Guid id, int quantity)
        {
            if (id != Guid.Empty)
            {
                Product product = productService.Get(id);

                if (product.Stock > 0 && quantity <= product.Stock)
                {
                    Sale sale = new Sale();
                    sale.ProductId = product.Id;
                    sale.MerchantUserName = product.MerchantUserName;
                    sale.UserName = User.Identity.Name;
                    sale.Date = DateTime.Now;
                    sale.Quantity = quantity;
                    sale.ProductName = product.Name;

                    saleService.Add(sale);
                    product.Stock -= quantity;

                    if (product.Stock <= 0) product.IsAvailable = false;
                    productService.Update(product);

                    Response.StatusCode = 200;
                    return Json(new { status = "success" });
                }

                Response.StatusCode = 404;
                return Json(new { status = "no stock", stock = product.Stock });
            }

            Response.StatusCode = 401;
            return Json(new { status = "unauth" });
        }

        [HttpPost]
        public IActionResult AddToCart(Guid id, int quantity)
        {
            if (id != Guid.Empty && quantity > 0)
            {
                var categories = categoryService.GetList();

                if (SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart") == null)
                {
                    List<CartItemViewModel> cart = new List<CartItemViewModel>();

                    Product item = productService.Get(id);

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

                    cart.Add(new CartItemViewModel { Product = viewModel, Quantity = quantity });

                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
                else
                {
                    List<CartItemViewModel> cart = SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart");

                    int index = isExist(id);
                    if (index != -1)
                    {
                        cart[index].Quantity += quantity;
                    }
                    else
                    {
                        Product item = productService.Get(id);

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

                        cart.Add(new CartItemViewModel { Product = viewModel, Quantity = quantity });
                    }

                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
            }

            Response.StatusCode = 200;
            return Json(new { status = "success" });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(Guid id)
        {
            if (id != Guid.Empty)
            {
                List<CartItemViewModel> cart = SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart");
                int index = isExist(id);
                cart.RemoveAt(index);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            }

            Response.StatusCode = 200;
            return Json(new { status = "success" });
        }

        private int isExist(Guid id)
        {
            List<CartItemViewModel> cart = SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        public IActionResult GetCartComponent()
        {
            return ViewComponent("CartNav");
        }
    }
}
