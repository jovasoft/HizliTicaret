using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class CategoryViewModel
    {
        [Required]
        public string Name { get; set; }
        public CategoryTypes CategoryType { get; set; }
    }
}
