using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models.ViewModels
{
    public class VeganDetailsVM
    {
        //Gives us a recipe of a specified id 
        public string Title { get; set; }
        public string RecImg { get; set; }
        public int RecId { get; set; }
        public bool IsSaved { get; set; }
        public string Username { get; set; }
        public string UserImg { get; set; }
        public string UserId { get; set; }
        public bool IsFollowing { get; set; }
        public bool IsUsersRecipe { get; set; }
        public string[] Categories { get; set; }
        public IngredientVM[] Ingredients { get; set; }
        public StepByStepVM[] Steps { get; set; }
    }

    public class IngredientVM
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
    }

    public class StepByStepVM
    {
        public string Instruction { get; set; }
        public int StepId { get; set; }
    }
}
