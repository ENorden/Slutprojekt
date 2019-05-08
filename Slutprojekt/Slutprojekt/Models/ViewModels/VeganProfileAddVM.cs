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
        [Display(Name = "Name of dish:")]
        [Required]
        public string Name { get; set; }

        [Required]
        public SelectListItem[] Categories { get; set; }

        [Required]
        public IFormFile Img { get; set; }

        [Required]
        public List<Ingredient> Ingredients { get; set; }

        [Required]
        public List<StepByStep> Steps { get; set; }

        [Required]
        [Display(Name = "Measure")]
        public SelectListItem[] MeasurementItems { get; set; }

        [Required]
        [Range(1, 5)]
        public string SelectedMeasurementValue { get; set; }



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
