using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class VisitManager : IVisitService
    {
        IVisitDal visitDal;

        public VisitManager(IVisitDal visitDal)
        {
            this.visitDal = visitDal;
        }

        public bool Add(Visit visit)
        {
            visitDal.Add(visit);
            return true;
        }

        public Visit Get(Guid visitId)
        {
            return visitDal.Get(x => x.Id == visitId);
        }

        public List<Visit> GetList()
        {
            return visitDal.GetList();
        }

        public bool IsVisitedToday(string ipAddress)
        {
            Visit visit = visitDal.Get(x => x.IPAddress == ipAddress && x.Date.Value.Day == DateTime.Now.Day);

            return (visit != null);
        }
    }
}
