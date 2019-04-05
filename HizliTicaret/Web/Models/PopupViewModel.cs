using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class PopupViewModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}
