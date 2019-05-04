using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public IActionResult AddRecipe()//VeganProfileVM profile
        {
            return View(service.GetAddedRecipe());//service.DisplayProfile(profile)
        }

        [HttpPost]
        [Route("profile/add")]
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
        public IActionResult SavedRecipes()
        {
            return View(service.DisplaySavedRecipes());
        }

        [Route("profile")]
        [Route("profile/post")]
        public IActionResult PostedRecipes()
        {
            return View(service.DisplayPosts());
        }

        [Route("details/{id}")]
        public IActionResult Details(int id)
        {
            return View(service.GetRecipesById(id));
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
        public async Task<IActionResult> LogOut()
        {
            await service.TryLogOutAsync();
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

        [Route("followers")]
        public IActionResult Followers()
        {
            return View(service.GetAllFollowers());
        }

        //[Route("SetRecipeImg")]
        //[AllowAnonymous]
        //public IActionResult SetRecipeImg(IFormFile file, int id)
        //{

        //    if (file?.Length > 0)
        //    {
        //      service.SaveImgToDB(file, id);
        //    }

        //    return null;
        //}

        //[Route("Posting")]
        //[HttpPost]
        //[AllowAnonymous]
        //public IActionResult PostToDatabase([FromBody] Foo viewModel)
        //{
        //    //service.SaveStepOne(categoryArray);
        //    return Ok();
        //}

        [Route("AddRecipieStep1")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddRecipieStep1(AddRecepieVM viewModel)
        {
            var recepieId = service.AddRecipieStep1(viewModel);
            return Json(recepieId);
        }
    }

    public class AddRecepieVM
    {
        public int RecepieId { get; set; }
        public string Title { get; set; }

        [Required(ErrorMessage ="You must choose at least one category")]
        public string CategoryIDs { get; set; }

        [Required(ErrorMessage = "You must upload a picture")]
        public IFormFile File { get; set; }
    }
}