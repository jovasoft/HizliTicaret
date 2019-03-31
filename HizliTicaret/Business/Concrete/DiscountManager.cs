using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class DiscountManager : IDiscountService
    {
        IDiscountDal discountDal;

        public DiscountManager(IDiscountDal discountDal)
        {
            this.discountDal = discountDal;
        }

        public bool Add(Discount discount)
        {
            discountDal.Add(discount);
            return true;
        }

        public void Delete(Guid discountId)
        {
            discountDal.Delete(discountDal.Get(x => x.Id == discountId));
        }

        public Discount Get(Guid discountId)
        {
            return discountDal.Get(x => x.Id == discountId);
        }

        public List<Discount> GetList()
        {
            return discountDal.GetList();
        }
    }
}
