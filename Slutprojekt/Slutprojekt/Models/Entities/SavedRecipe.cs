using System;
using System.Collections.Generic;

namespace Slutprojekt.Models.Entities
{
    public partial class SavedRecipe
    {
        public int Id { get; set; }
        public int RecId { get; set; }
        public string UserId { get; set; }

        public virtual Recipe Rec { get; set; }
    }
}
