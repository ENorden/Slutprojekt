using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models.ViewModels
{
    public class VeganRecipesVM
    {
        public List<string> Categories { get; set; } = new List<string>
        {
            "Breakfast",
            "Lunch",
            "Dinner",
        };

        public string Pasta { get; set; }


    }
}
