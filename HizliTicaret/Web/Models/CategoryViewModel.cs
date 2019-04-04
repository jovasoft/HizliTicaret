using Entities.Concrete;
using Microsoft.AspNetCore.Http;
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
        public Guid MainCategoryId { get; set; }
        public IFormFile File { get; set; }
        public Guid Id { get; set; }
    }
}
