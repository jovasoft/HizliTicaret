using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Abstract
{
    public interface IProductService
    {
        bool Add(Product product);

        Product Get(Guid productId);

        List<Product> GetList(Expression<Func<Product, bool>> filter = null);

        void Delete(Guid productId);
    }
}
