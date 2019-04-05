using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class PopupManager : IPopupService
    {
        IPopupDal popupDal;

        public PopupManager(IPopupDal popupDal)
        {
            this.popupDal = popupDal;
        }

        public bool Add(Popup popup)
        {
            popupDal.Add(popup);
            return true;
        }

        public void Delete(Guid popupId)
        {
            popupDal.Delete(popupDal.Get(x => x.Id == popupId));
        }

        public Popup Get(Guid popupId)
        {
            return popupDal.Get(x => x.Id == popupId);
        }

        public List<Popup> GetList()
        {
            return popupDal.GetList();
        }

        public bool Update(Popup popup)
        {
            popupDal.Update(popup);

            return true;
        }
    }
}
