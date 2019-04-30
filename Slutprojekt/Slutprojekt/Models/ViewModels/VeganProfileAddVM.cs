using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models.ViewModels
{
    public class VeganProfileAddVM
    {
        public string Name { get; set; }
        public List<string> Categories { get; set; }
        public IFormFile Img { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<StepByStep> Steps { get; set; }


        public class Ingredient
        {
            public string Name { get; set; }
            public double Amount { get; set; }
            public string Measure { get; set; }
        }

        public class StepByStep
        {
            public int SortOrder { get; set; }
            public string Instruction { get; set; }
        }

    }
}
