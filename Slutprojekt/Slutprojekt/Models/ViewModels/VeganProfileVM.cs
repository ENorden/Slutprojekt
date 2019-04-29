using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models.ViewModels
{
    public class VeganProfileVM
    {
        public string UserName { get; set; }
        public int Posts { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }


            
    }
}
