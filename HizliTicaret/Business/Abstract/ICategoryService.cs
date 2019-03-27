using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        bool Add(Category category);

        Category Get(Guid categoryId);

        List<Category> GetList();

        void Delete(Guid categoryId);

        Category GetMainCategory(CategoryTypes categoryTypes);
    }
}
