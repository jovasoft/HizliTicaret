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
        public Guid BrandId { get; set; }
        public Guid UserId { get; set; }
    }
}
