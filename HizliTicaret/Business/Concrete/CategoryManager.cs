using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            this.categoryDal = categoryDal;
        }

        public bool Add(Category category)
        {
            categoryDal.Add(category);
            return true;
        }

        public void Delete(Guid categoryId)
        {
            categoryDal.Delete(categoryDal.Get(x => x.Id == categoryId));
        }

        public Category Get(Guid categoryId)
        {
            return categoryDal.Get(x => x.Id == categoryId);
        }

        public List<Category> GetList()
        {
            return categoryDal.GetList();
        }
    }
}
