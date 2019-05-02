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

        public VeganRecipeVM[] GetAllCategories()
        {
            return context.Category
                .Select(c => new VeganRecipeVM
                {
                     Img = c.Img,
                     CategoryName = c.CategoryName,
                     Id = c.Id
                })
                .ToArray();
        }

        public VeganCategoryVM[] GetRecipesByCategory(int id)
        {
            var recipes = context.Category
                .Where(c => c.Id == id)
                .SelectMany(c => c.Recipe2Category.Select(r => new VeganCategoryVM
                {
                    Id = r.RecId,
                    Img = r.Rec.Img,
                    Title = r.Rec.Title,
                    UserId = r.Rec.UserId
                }))
                .ToArray();

            return recipes;
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
            profile.Categories = new SelectListItem[] {
                    new SelectListItem { Value = "1", Text = "Lunch" },
                    new SelectListItem { Value = "2", Text = "Dinner", Selected = true },
                    new SelectListItem { Value = "3", Text = "Dessert" },
                };
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

        internal void SaveStepOne(string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                context.Category.Add(new Category
                {
                    CategoryName = array[i]
                });
            }

            context.SaveChanges();
        }
    }
}
