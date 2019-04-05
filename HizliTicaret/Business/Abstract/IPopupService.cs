using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IPopupService
    {

        bool Add(Popup popup);

        Popup Get(Guid popupId);

        List<Popup> GetList();

        void Delete(Guid popupId);

        bool Update(Popup category);
    }
}
