using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models.ViewModels
{
    public class VeganSavedVM
    {
        public string Username { get; set; }
        public string ProfileImg { get; set; }
        public PostItemVM3[] Posts { get; set; }
    }

    public class PostItemVM3
    {
        public string RecipeTitle { get; set; }
        public string[] RecipeCategories { get; set; }
        public string RecipeImg { get; set; }
        //public bool IsSaved { get; set; }
    }

}
