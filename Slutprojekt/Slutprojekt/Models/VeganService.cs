using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Slutprojekt.Controllers;
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
        readonly IHttpContextAccessor accessor;

        public VeganService(
            UserManager<VeganIdentityUser> userManager,
            SignInManager<VeganIdentityUser> signInManager,
            SlutprojektContext context,
            IHttpContextAccessor accessor
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.accessor = accessor;
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

        internal async Task<VeganFollowersVM[]> GetAllFollowersAsync()
        {
            // Hämta den inloggade användarens ID (från auth-cookie)
            string userId = userManager.GetUserId(accessor.HttpContext.User);

            var followers = context.Follower
                .Where(u => u.UserId == userId)
                    .Select(f => new VeganFollowersVM
                    {
                        Username = f.User.UserName,
                        ProfileImg = f.User.PictureUrl,
                        Posts = f.User.Recipe.Select(r => new PostItemVM
                        {
                            RecipeTitle = r.Title,
                            RecipeImg = r.Img
                        })
                        .ToArray()
                    })
                .ToArray();

            return followers;

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
            string currentCategory = context.Category
                .First(c => c.Id == id).CategoryName;

            var recipes = context.Category
                .Where(c => c.Id == id)
                .SelectMany(c => c.Recipe2Category.Select(r => new VeganCategoryVM
                {
                    Id = r.RecId,
                    Img = r.Rec.Img,
                    Title = r.Rec.Title,
                    UserId = r.Rec.UserId,
                    CategoryName = currentCategory
                }))
                .ToArray();

            return recipes;
        }

        public VeganFollowersVM[] DisplayPosts()
        {
            string userId = userManager.GetUserId(accessor.HttpContext.User);

            var posts = context.Follower
                .Where(u => u.UserId == userId)
                    .Select(u => new VeganFollowersVM
                    {
                        Username = u.User.UserName,
                        ProfileImg = u.User.PictureUrl,
                        Posts = u.User.Recipe.Select(r => new PostItemVM
                        {
                            RecipeTitle = r.Title,
                            RecipeImg = r.Img
                        })
                        .ToArray()
                    })
                .ToArray();

            return posts;
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
            profile.Categories = context.Category
                .Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.CategoryName })
                .ToArray();
            return profile;
        }

        public async Task<VeganProfileVM> GetProfileInfoAsync()
        {
            // Hämta den inloggade användarens ID (från auth-cookie):
            string userId = userManager.GetUserId(accessor.HttpContext.User);

            VeganProfileVM viewModel = context.AspNetUsers
                .Where(u => u.Id == userId)
                .Select(u => new VeganProfileVM
                {
                    Description = u.Description,
                    UserName = u.UserName,
                    PictureURL = u.PictureUrl,
                    Posts = u.Recipe.Count,
                    Followers = u.FollowerFollowerNavigation.Count,
                    Following = u.FollowerUser.Count

                })
                .SingleOrDefault();

            return viewModel;

        }

        //internal void SaveImgToDB(IFormFile file, int id)
        //{
        //    var fileName = Path.GetFileName(file.FileName);
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", fileName);
        //    using (var fileSrteam = new FileStream(filePath, FileMode.Create))
        //    {
        //        file.CopyToAsync(fileSrteam);
        //    }

        //    VeganRecipeVM recipe = context.Recipe
        //        .Where(r => r.Id == id)
        //        .Select(r => new VeganRecipeVM
        //        {
        //            Img = filePath
        //        })
        //        .SingleOrDefault();

        //    //var temp = new Recipe
        //    //{
        //    //    Img = filePath
        //    //};

        //    //context.Recipe.Add(temp);
        //    //context.SaveChanges();
        //    //var id = temp.Id;

        //    //return id;
        //}

        public int AddRecipieStep1(AddRecepieVM viewModel)
        {
            Recipe recipe;

            if (viewModel.RecepieId == 0)
            {
                string userId = userManager.GetUserId(accessor.HttpContext.User);
                recipe = new Recipe();
                recipe.UserId = userId;
                recipe.Title = "Temp";
                recipe.Img = "Temp";
                context.Recipe.Add(recipe);
                context.SaveChanges();
                viewModel.RecepieId = recipe.Id;
            }
            else
            {
                recipe = context.Recipe.Find(viewModel.RecepieId);
            }

            var fileName = Path.GetFileName(viewModel.File.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", fileName);
            using (var fileSrteam = new FileStream(filePath, FileMode.Create))
            {
                viewModel.File.CopyToAsync(fileSrteam);
            }

            recipe.Title = viewModel.Title;
            recipe.Img = viewModel.File.FileName;

            recipe.Recipe2Category.Clear();

            var array = viewModel.CategoryIDs.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                context.Recipe2Category.Add(new Recipe2Category
                {
                    CatId = int.Parse(array[i]),
                    RecId = viewModel.RecepieId

                });
            }

            context.SaveChanges();
            return viewModel.RecepieId;
        }
    }
}
