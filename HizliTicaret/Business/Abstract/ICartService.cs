﻿using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Abstract
{
    public interface ICartService
    {
        bool Add(Cart cart);

        Cart Get(Guid cartId);

        List<Cart> GetList();

        void Delete(Guid cartId);
    }
}