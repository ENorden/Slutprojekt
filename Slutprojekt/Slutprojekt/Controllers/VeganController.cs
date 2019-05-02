using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Slutprojekt.Models;
using Slutprojekt.Models.ViewModels;

namespace Slutprojekt.Controllers
{
    [Authorize]
    public class VeganController : Controller
    {
        VeganService service;

        private IHostingEnvironment _Hostenv { get; }

        public VeganController(VeganService service, IHostingEnvironment _hostenv)
        {
            this.service = service;
            _Hostenv = _hostenv;
        }

        [HttpGet]
        [Route("profile/add")]
        [Route("")]
        [AllowAnonymous]
        public IActionResult AddRecipe()//VeganProfileVM profile
        {
            return View(service.GetAddedRecipe());//service.DisplayProfile(profile)
        }

        [HttpPost]
        [Route("profile/add")]
        [AllowAnonymous]
        public IActionResult AddRecipe(VeganProfileAddVM viewModel)
        {
            if (viewModel.Img?.Length > 0)
            {
                // IHostingEnvironment was injected into the controller
                var filePath = Path.Combine(_Hostenv.WebRootPath,
                    "Uploads", viewModel.Img.FileName);

                // Save file to disk
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    viewModel.Img.CopyTo(fileStream);
                }
            }

            return RedirectToAction(nameof(Register));
        }



        [Route("profile/save")]
        [AllowAnonymous]
        public IActionResult SaveRecipe()//VeganProfileVM profile
        {
            return View();//service.DisplayProfile(profile)
        }

        [Route("profile/post")]
        [AllowAnonymous]
        public IActionResult PostRecipe()//VeganProfileVM profile
        {
            return View();//service.DisplayProfile(profile)
        }

        [HttpGet]
        [Route("register")]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(VeganRegisterVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            // Try to register user
            var result = await service.TryRegisterAsync(viewModel);
            if (!result.Succeeded)
            {
                // Show error
                ModelState.AddModelError(string.Empty, result.Errors.First().Description);
                return View(viewModel);
            }

            // Redirect user
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        //[Route("")]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new VeganLoginVM { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(VeganLoginVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            // Check if credentials is valid (and set auth cookie)
            var result = await service.TryLoginAsync(viewModel);
            if (!result.Succeeded)
            {
                // Show error
                ModelState.AddModelError(nameof(VeganLoginVM.Username), "Login failed");
                return View(viewModel);
            }

            // Redirect user
            if (string.IsNullOrWhiteSpace(viewModel.ReturnUrl))
                return RedirectToAction(nameof(Recipes));
            else
                return Redirect(viewModel.ReturnUrl);
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult LogOut()
        {
            service.TryLogOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [Route("recipes")]
        public IActionResult Recipes()
        {
            return View(service.GetAllCategories());
        }

        [Route("recipes/{id}")]
        public IActionResult Category(int id)
        {
            return View(service.GetRecipesByCategory(id));
        }

        [HttpGet]
        [Route("followers")]
        public IActionResult Followers(VeganFollowersVM followersVM)
        {
            return View();
        }

        [Route("Image/File")]
        [AllowAnonymous]
        public IActionResult FileUpload(IFormFile file)
        {
            // Always check content length
            if (file?.Length > 0)
            {
                service.SaveImgToDB(file);

            }

            return null;
        }
                
        [Route("Posting")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult PostToDatabase([FromBody] Foo viewModel)
        {
            //service.SaveStepOne(categoryArray);
            return Ok();
        }
    }

    public class Foo
    {
        public int Age { get; set; }
        public int[] CategoryIDs { get; set; }
    }
}