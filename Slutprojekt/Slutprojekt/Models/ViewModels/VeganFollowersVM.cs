using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models.ViewModels
{
    public class VeganFollowersVM
    {
        //Är det rätt nedan? osäker
        public List<VeganFollowersVM> Followers;
        public string FullName { get; set; }
        public string Username { get; set; }
        public List<string> Posts { get; set; }
    }
}
