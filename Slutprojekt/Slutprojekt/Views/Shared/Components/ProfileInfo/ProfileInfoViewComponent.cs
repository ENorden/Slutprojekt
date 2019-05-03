using Microsoft.AspNetCore.Mvc;
using Slutprojekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Views.Shared.Components
{
    public class ProfileInfoViewComponent : ViewComponent
    {
        private VeganService service;

        public ProfileInfoViewComponent(VeganService service)
        {
            this.service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await service.GetProfileInfoAsync());
        }
    }
}
