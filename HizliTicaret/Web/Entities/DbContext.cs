using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Web
{
    public class DbContext : IdentityDbContext<User, Role, Guid>
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
            
        }
    }
}