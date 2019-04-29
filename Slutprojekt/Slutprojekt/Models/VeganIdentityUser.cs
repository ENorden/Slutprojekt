using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models
{
    public class VeganIdentityUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
