using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class DiscountViewModel
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid MainCategoryId { get; set; }
        public int Percent { get; set; }
    }
}
