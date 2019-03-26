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

        public ProductController(IFavoriteService favoriteService, IProductService productService)
        {
            this.favoriteService = favoriteService;
            this.productService = productService;
        }

        public IActionResult Show(Guid id)
        {
            Product product = productService.Get(id);

            return View(product);
        }

        [HttpPost]
        public IActionResult AddFavorite(Guid id)
        {
            if (id != null)
            {
                Favorite favorite = new Favorite();
                favorite.ProductId = id;
                favorite.UserName = User.Identity.Name;

                favoriteService.Add(favorite);
            }

            return View();
        }
    }
}
