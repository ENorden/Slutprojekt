using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models
{
    public class VeganIdentityUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public string Description { get; set; }

    }
}
