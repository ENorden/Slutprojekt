using System;
using System.Collections.Generic;

namespace Slutprojekt.Models.Entities
{
    public partial class Recipe
    {
        public Recipe()
        {
            Ingredient = new HashSet<Ingredient>();
            Recipe2Category = new HashSet<Recipe2Category>();
            SavedRecipe = new HashSet<SavedRecipe>();
            StepByStep = new HashSet<StepByStep>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<Ingredient> Ingredient { get; set; }
        public virtual ICollection<Recipe2Category> Recipe2Category { get; set; }
        public virtual ICollection<SavedRecipe> SavedRecipe { get; set; }
        public virtual ICollection<StepByStep> StepByStep { get; set; }
    }
}
