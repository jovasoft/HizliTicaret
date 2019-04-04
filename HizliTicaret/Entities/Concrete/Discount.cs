using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Discount : IEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public int Percent { get; set; }
        public string MerchantUserName { get; set; }
    }
}
