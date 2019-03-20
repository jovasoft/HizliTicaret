using Business.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class SaleManager : ISaleService
    {
        public bool Add(Sale sale)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid saleId)
        {
            throw new NotImplementedException();
        }

        public Sale Get(Guid saleId)
        {
            throw new NotImplementedException();
        }

        public List<Sale> GetList(Expression<Func<Sale, bool>> filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
