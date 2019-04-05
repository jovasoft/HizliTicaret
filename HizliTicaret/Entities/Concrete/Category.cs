using Core;
using System;

namespace Entities.Concrete
{
    public class Category : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CategoryTypes CategoryType { get; set; }
        public Guid MainCategoryId { get; set; }
        public string ImageUrl { get; set; }
        public int Discounts { get; set; }
        public int SoldCount { get; set; }
    }

    public enum CategoryTypes
    {
        Women,
        Men,
        Kid,
    }
}
