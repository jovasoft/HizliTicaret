using Business.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class BrandManager : IBrandService
    {
        public bool Add(Brand brand)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid brandId)
        {
            throw new NotImplementedException();
        }

        public Brand Get(Guid brandId)
        {
            throw new NotImplementedException();
        }

        public List<Brand> GetList(Expression<Func<Brand, bool>> filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
