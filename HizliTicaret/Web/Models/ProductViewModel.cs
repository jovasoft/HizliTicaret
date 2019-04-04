using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class ProductViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public Category Category { get; set; }
        public string MainCategory { get; set; }
        public string MerchantUserName { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; }
        public int Discounts { get; set; }
        public IFormFile File { get; set; }
        public decimal PriceWithDiscounts { get; set; }
        public decimal LastPrice { get { return PriceWithDiscounts - ((PriceWithDiscounts * Category.Discounts) / 100); } }
        public decimal DiscountTotal { get { return LastPrice - Price; } }
    }
}
