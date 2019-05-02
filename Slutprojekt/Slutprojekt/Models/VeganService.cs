using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Slutprojekt.Models.Entities;
using Slutprojekt.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models
{
    public class VeganService
    {
        UserManager<VeganIdentityUser> userManager;
        SignInManager<VeganIdentityUser> signInManager;
        readonly SlutprojektContext context;

        public VeganService(
            UserManager<VeganIdentityUser> userManager,
            SignInManager<VeganIdentityUser> signInManager,
            SlutprojektContext context
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        public async Task<IdentityResult> TryRegisterAsync(VeganRegisterVM viewModel)
        {
            // Try to create a new user
            return await userManager.CreateAsync(
                new VeganIdentityUser { UserName = viewModel.Username, Email = viewModel.Email, FirstName = viewModel.FirstName, LastName = viewModel.LastName },
                viewModel.Password);
        }

        public async Task<SignInResult> TryLoginAsync(VeganLoginVM viewModel)
        {
            // Try to sign user
            return await signInManager.PasswordSignInAsync(
                viewModel.Username,
                viewModel.Password,
                isPersistent: false,
                lockoutOnFailure: false);
        }

        public async Task TryLogOutAsync()
        {
            await signInManager.SignOutAsync();
        }

        internal VeganFollowersVM[] GetAllFollowers(VeganFollowersVM followersVM)
        {
            return null;
            //return context.Users
            //    .Select(person => new VeganFollowersVM
            //    {
            //        Username = person.UserName,
            //        FirstName = person.FirstName,
            //        Posts = { "Hello", "My recipe" }
            //    })
            //    .ToArray();
        }

        public VeganRecipeVM GetAllCategories()
        {
            VeganRecipeVM test = new VeganRecipeVM();
            return test;
        }

        public string DisplayProfile(VeganProfileVM profile)
        {
            throw new NotImplementedException();
        }

        public VeganProfileAddVM GetAddedRecipe()
        {
            
            VeganProfileAddVM profile = new VeganProfileAddVM()
            {
                MeasurementItems = new SelectListItem[]
                {
                    new SelectListItem { Value = "1", Text = "Dl" },
                    new SelectListItem { Value = "2", Text = "Msk", Selected = true },
                    new SelectListItem { Value = "3", Text = "Tsk" },
                }
            };
            profile.Categories = new List<string> { "Lunch", "Dinner", "Dessert" };
            return profile;
        }

        internal void SaveImgToDB(IFormFile file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", fileName);
            using (var fileSrteam = new FileStream(filePath, FileMode.Create))
            {
                file.CopyToAsync(fileSrteam);
            }

            context.Recipe.Add(new Recipe
            {
                Img = filePath
            });
            context.SaveChanges();
        }
    }
}
