using Core;
using System;

namespace Entities.Concrete
{
    public class Brand : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
