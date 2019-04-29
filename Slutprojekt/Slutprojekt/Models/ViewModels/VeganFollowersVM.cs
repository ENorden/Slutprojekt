using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models.ViewModels
{
    public class VeganFollowersVM
    {
        public string FirstName { get; set; }
        public string Username { get; set; }
        public List<string> Posts { get; set; }
    }
}
