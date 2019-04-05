using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Popup : IEntity
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }
}
