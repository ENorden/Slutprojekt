using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Measure")]
        public SelectListItem[] MeasurementItems { get; set; }

        [Range(1, 3)]
        public int SelectedMeasurementValue { get; set; }



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
