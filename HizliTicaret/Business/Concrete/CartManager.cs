using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class CartManager : ICartService
    {
        ICartDal cartDal;

        public CartManager(ICartDal cartDal)
        {
            this.cartDal = cartDal;
        }

        public bool Add(Cart cart)
        {
            cartDal.Add(cart);
            return true;
        }

        public void Delete(Guid cartId)
        {
            cartDal.Delete(cartDal.Get(x => x.Id == cartId));
        }

        public Cart Get(Guid cartId)
        {
            return cartDal.Get(x => x.Id == cartId);
        }

        public List<Cart> GetList()
        {
            return cartDal.GetList();
        }
    }
}
