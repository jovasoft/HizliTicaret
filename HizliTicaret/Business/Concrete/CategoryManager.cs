using Business.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        public bool Add(Category category)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Category Get(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public List<Category> GetList(Expression<Func<Category, bool>> filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
