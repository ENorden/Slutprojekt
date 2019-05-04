using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models.ViewModels
{
    public class VeganSavedVM
    {
        public string CreatorUsername { get; set; }
        public string CreatorProfileImg { get; set; }
        public string RecipeTitle { get; set; }
        public string[] RecipeCategories { get; set; }
        public string RecipeImg { get; set; }
    }

}
