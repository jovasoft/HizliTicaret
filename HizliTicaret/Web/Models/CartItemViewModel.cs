using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class CartItemViewModel
    {
        public ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
    }
}
