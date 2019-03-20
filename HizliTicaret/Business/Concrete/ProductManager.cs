using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal productDal;

        public ProductManager(IProductDal productDal)
        {
            this.productDal = productDal;
        }

        public bool Add(Product product)
        {
            productDal.Add(product);
            return true;
        }

        public void Delete(Guid productId)
        {
            productDal.Delete(productDal.Get(x => x.Id == productId));
        }

        public Product Get(Guid productId)
        {
            return productDal.Get(x => x.Id == productId);
        }

        public List<Product> GetList()
        {
            return GetList();
        }
    }
}
