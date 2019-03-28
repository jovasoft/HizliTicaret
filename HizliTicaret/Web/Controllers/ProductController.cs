using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        IFavoriteService favoriteService;
        IProductService productService;
        ICategoryService categoryService;

        public ProductController(IFavoriteService favoriteService, IProductService productService, ICategoryService categoryService)
        {
            this.favoriteService = favoriteService;
            this.productService = productService;
            this.categoryService = categoryService;
        }

        public IActionResult Show(Guid id)
        {
            Product product = productService.Get(id);
            product.MerchantUserName = "tester@gmail.com";
            Category category = categoryService.Get(product.CategoryId);
            Category mainCat = categoryService.GetMainCategory(CategoryTypes.Women);
            bool isFavorited = favoriteService.GetList(User.Identity.Name).Exists(x => x.ProductId == product.Id);

            ViewData["ProductCategoryName"] = category.Name;
            ViewData["ProductMainCategoryName"] = mainCat.Name;
            ViewData["IsFavorited"] = isFavorited;

            return View(product);
        }

        [HttpPost]
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
    }
}
