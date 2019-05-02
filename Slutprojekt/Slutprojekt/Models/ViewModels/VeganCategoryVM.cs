using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models.ViewModels
{
    public class VeganCategoryVM
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
    }
}
