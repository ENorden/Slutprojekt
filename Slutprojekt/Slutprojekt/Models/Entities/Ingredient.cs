using System;
using System.Collections.Generic;

namespace Slutprojekt.Models.Entities
{
    public partial class Ingredient
    {
        public int Id { get; set; }
        public int RecId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }

        public virtual Recipe Rec { get; set; }
    }
}
