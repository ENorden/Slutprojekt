using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models.ViewModels
{
    public class VeganPostVM
    {
        public string RecipeTitle { get; set; }
        public string[] RecipeCategories { get; set; }
        public string RecipeImg { get; set; }
        public int RecipeId { get; set; }
        public string UserImg { get; set; }
        public string Username { get; set; }

    }

}
