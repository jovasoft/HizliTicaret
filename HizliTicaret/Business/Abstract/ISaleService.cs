using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Abstract
{
    public interface ISaleService
    {
        bool Add(Sale sale);

        Sale Get(Guid saleId);

        List<Sale> GetList();

        void Delete(Guid saleId);
    }
}
