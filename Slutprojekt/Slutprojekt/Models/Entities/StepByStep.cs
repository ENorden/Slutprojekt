using System;
using System.Collections.Generic;

namespace Slutprojekt.Models.Entities
{
    public partial class StepByStep
    {
        public int Id { get; set; }
        public int RecId { get; set; }
        public string Instruction { get; set; }
        public int SortOrder { get; set; }

        public virtual Recipe Rec { get; set; }
    }
}
