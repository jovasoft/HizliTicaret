using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class BrandManager : IBrandService
    {
        private IBrandDal brandDal;

        public BrandManager(IBrandDal brandDal)
        {
            this.brandDal = brandDal;
        }

        public bool Add(Brand brand)
        {
            brandDal.Add(brand);
            return true;
        }

        public void Delete(Guid brandId)
        {
            brandDal.Delete(brandDal.Get(x => x.Id == brandId));
        }

        public Brand Get(Guid brandId)
        {
            return brandDal.Get(x => x.Id == brandId);
        }

        public List<Brand> GetList()
        {
            return brandDal.GetList();
        }
    }
}
