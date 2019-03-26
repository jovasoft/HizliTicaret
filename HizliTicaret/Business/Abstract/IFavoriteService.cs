using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Abstract
{
    public interface IFavoriteService
    {
        bool Add(Favorite favorite);

        Favorite Get(Guid favoriteId);

        List<Favorite> GetList();

        List<Favorite> GetList(string username);

        void Delete(Guid favoriteId);
    }
}
