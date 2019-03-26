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
    public class FavoriteManager : IFavoriteService
    {
        IFavoriteDal favoriteDal;

        public FavoriteManager(IFavoriteDal favoriteDal)
        {
            this.favoriteDal = favoriteDal;
        }

        public bool Add(Favorite favorite)
        {
            favoriteDal.Add(favorite);
            return true;
        }
        
        public void Delete(Guid favoriteId)
        {
            favoriteDal.Delete(favoriteDal.Get(x => x.Id == favoriteId));
        }

        public Favorite Get(Guid favoriteId)
        {
            return favoriteDal.Get(x => x.Id == favoriteId);
        }

        public List<Favorite> GetList()
        {
            return favoriteDal.GetList();
        }

        public List<Favorite> GetList(string username)
        {
           return favoriteDal.GetList(x => x.UserName == username);
        }
        
    }
}
