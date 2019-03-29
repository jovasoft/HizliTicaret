using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Sale : IEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string MerchantUserName { get; set; }
        public string UserName { get; set; }
        public DateTime? Date { get; set; }
        public int Quantity { get; set; }
    }
}
