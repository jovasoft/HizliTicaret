using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IDiscountService
    {
        bool Add(Discount discount);

        Discount Get(Guid discountId);

        List<Discount> GetList();

        void Delete(Guid discountId);
    }
}
