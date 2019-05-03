using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models.ViewModels
{
    public class VeganFollowersVM
    {
        public string Username { get; set; }
        public string ProfileImg { get; set; }
        public string RecipeTitle { get; set; }
        public string RecipeCategory { get; set; }
        public string Description { get; set; }
        public bool IsSaved { get; set; }
    }
}
