using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Abstract
{
    public interface IBrandService
    {
        bool Add(Brand brand);

        Brand Get(Guid brandId);

        List<Brand> GetList();

        void Delete(Guid brandId);
    }
}
