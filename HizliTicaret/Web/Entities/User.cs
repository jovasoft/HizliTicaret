using Microsoft.AspNetCore.Identity;
using System;

namespace Web
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public DateTime? RegisteredDate { get; set; }
    }
}