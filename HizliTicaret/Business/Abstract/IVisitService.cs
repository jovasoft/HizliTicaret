using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Abstract
{
    public interface IVisitService
    {
        bool Add(Visit visit);

        Visit Get(Guid visitId);

        List<Visit> GetList();

        bool IsVisitedToday(string ipAddress);
    }
}
