using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models.ViewModels
{
    public class VeganEditProfileVM
    {
        public string UserName { get; set; }
        public string Description { get; set; }

        [Display(Name = "Profile picture")]
        public IFormFile PictureURL { get; set; }


    }
}
