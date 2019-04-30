using System;
using System.Collections.Generic;

namespace Slutprojekt.Models.Entities
{
    public partial class Category
    {
        public Category()
        {
            Recipe2Category = new HashSet<Recipe2Category>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Recipe2Category> Recipe2Category { get; set; }
    }
}
