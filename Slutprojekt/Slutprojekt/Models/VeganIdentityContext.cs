using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models
{
    public class VeganIdentityContext : IdentityDbContext<VeganIdentityUser>
    {
        public VeganIdentityContext(DbContextOptions<VeganIdentityContext> options)
            : base(options)
        {
            // Create DB schema (first time)
            var result = Database.EnsureCreated();
        }
    }
}
