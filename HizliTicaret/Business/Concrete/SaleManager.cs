using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class SaleManager : ISaleService
    {
        ISaleDal saleDal;

        public SaleManager(ISaleDal saleDal)
        {
            this.saleDal = saleDal;
        }

        public bool Add(Sale sale)
        {
            saleDal.Add(sale);
            return true;
        }

        public void Delete(Guid saleId)
        {
            saleDal.Delete(saleDal.Get(x => x.Id == saleId);
        }

        public Sale Get(Guid saleId)
        {
            return saleDal.Get(x => x.Id == saleId);
        }

        public List<Sale> GetList()
        {
            return GetList();
        }
    }
}
