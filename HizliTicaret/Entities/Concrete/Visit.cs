using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Visit : IEntity
    {
        public Guid Id { get; set; }
        public string IPAddress { get; set; }
        public DateTime? Date { get; set; }
    }
}
