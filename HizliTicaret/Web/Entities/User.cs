using Microsoft.AspNetCore.Identity;
using System;

namespace Web
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool IsMerchant { get; set; }
    }
}