using Business.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class CartManager : ICartService
    {
        public bool Add(Cart cart)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid cartId)
        {
            throw new NotImplementedException();
        }

        public Cart Get(Guid cartId)
        {
            throw new NotImplementedException();
        }

        public List<Cart> GetList(Expression<Func<Cart, bool>> filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
