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
        [HttpGet]
        public IActionResult PostedRecipes()
        {
            return View(service.DisplayPosts());
        }

        [HttpGet]
        [Route("editprofile")]
        public async Task<IActionResult> EditProfile()
        {
            return View(await service.GetUserAsync());
        }

        [Route("editprofile")]
        [HttpPost]
        public async Task<IActionResult> EditProfile(VeganEditProfileVM viewModel)
        {
            if (viewModel.PictureURL?.Length > 0)
            {
                // IHostingEnvironment was injected into the controller
                var filePath = Path.Combine(_Hostenv.WebRootPath,
                    "Uploads", viewModel.PictureURL.FileName);

                // Save file to disk
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    viewModel.PictureURL.CopyTo(fileStream);
                }
            }

            if (!ModelState.IsValid)
                return View(viewModel);

            await service.UpdateUserProfileAsync(viewModel);
            return RedirectToAction(nameof(PostedRecipes));
        }

        [Route("details/{id:int}")]
        public IActionResult Details(int id)
        {
            return View(service.GetRecipesById(id));
        }

        [HttpGet]
        [Route("Unsave/{id}")]
        public IActionResult Unsave(int id)
        {
            return Content(service.UnsaveRecipe(id));
        }

        [HttpGet]
        [Route("Save/{id}")]
        public IActionResult Save(int id)
        {
            return Content(service.SaveRecipe(id));
        }

        [HttpGet]
        [Route("Unfollow/{id}")]
        public IActionResult Unfollow(string id)
        {
            return Content(service.UnfollowPerson(id));
        }

        [HttpGet]
        [Route("Follow/{id}")]
        public IActionResult Follow(string id)
        {
            return Content(service.FollowPerson(id));
        }

        [Route("DeletePost/{id:int}")]
        public IActionResult DeletePost(int id)
        {
            service.DeletePost(id);
            return Ok();
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
        public async Task<IActionResult> AddRecipieStep1(AddRecepieVM viewModel)
        {
            var recepieId = await service.AddRecipieStep1(viewModel);
            return Json(recepieId);
        }

        [Route("AddRecipieStep2")]
        [HttpPost]
        public IActionResult AddRecipieStep2(StepTwo stepTwo)
        {

            var ingrID = service.AddRecipieStep2(stepTwo);
            return Json(ingrID); 
        }

        [Route("AddRecipieStep3")]
        [HttpPost]
        public IActionResult AddRecipieStep3(Textbox description)
        {

            service.AddRecipieStep3(description);
            return Ok();
        }

        [Route("DeleteIngredient")]
        [HttpPost]
        public IActionResult DeleteIngredient(DeleteIng delete)
        {
            service.DeleteIngredient(delete);
            return Ok();
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

    public class StepTwo
    {
        public string RecipeIngr { get; set; }
        public double RecipeAmount { get; set; }
        public string DropDownVal { get; set; }
        public int RecID { get; set; }

    }

    public class Textbox
    {
        public string TextBox { get; set; }
        public int RecID { get; set; }

    }

    public class DeleteIng
    {
        public int CurrentIngID { get; set; }

    }
}