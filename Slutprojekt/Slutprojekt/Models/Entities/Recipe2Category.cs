using System;
using System.Collections.Generic;

namespace Slutprojekt.Models.Entities
{
    public partial class Recipe2Category
    {
        public int Id { get; set; }
        public int RecId { get; set; }
        public int CatId { get; set; }

        public virtual Category Cat { get; set; }
        public virtual Recipe Rec { get; set; }
    }
}
