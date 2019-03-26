using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.ViewComponents
{
    public class CategoryNav : ViewComponent
    {
        ICategoryService categoryService;

        public CategoryNav(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
         
        public IViewComponentResult Invoke()
        {
            /*
            //adding main category
            Category category1 = new Category();
            category1.CategoryType = CategoryTypes.Women;
            category1.Name = "Kadın";
            category1.MainCategoryId = null;

            Category category2 = new Category();
            category2.CategoryType = CategoryTypes.Men;
            category2.Name = "Erkek";
            category2.MainCategoryId = null;

            Category category3 = new Category();
            category3.CategoryType = CategoryTypes.Kid;
            category3.Name = "Çocuk";
            category3.MainCategoryId = null;

            categoryService.Add(category1);
            categoryService.Add(category2);
            categoryService.Add(category3);
            */

            var categories = categoryService.GetList();
            return View(categories);
        }
    }
}
