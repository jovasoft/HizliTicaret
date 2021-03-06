﻿using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Product : IEntity
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string MerchantUserName { get; set; }
        public decimal Price { get; set; }
        public int Discounts { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public decimal PriceWithDiscounts { get { return Price - (Price * Discounts) / 100; } }
        public int SoldCount { get; set; }
        public int AddedToCartCount { get; set; }
    }
}
