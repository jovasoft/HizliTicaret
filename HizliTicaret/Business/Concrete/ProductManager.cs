using Business.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        public bool Add(Product product)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Product Get(Guid productId)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetList(Expression<Func<Product, bool>> filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
