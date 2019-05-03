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

        public VeganPostVM DisplayPosts()
        {
            string userId = userManager.GetUserId(accessor.HttpContext.User);

            var posts = context.AspNetUsers
                .Where(u => u.Id == userId)
                    .Select(u => new VeganPostVM
                    {
                        Username = u.UserName,
                        ProfileImg = u.PictureUrl,
                        Posts = u.Recipe.Select(r => new PostItemVM2
                        {
                            RecipeTitle = r.Title,
                            RecipeImg = r.Img,
                            RecipeCategories = r.Recipe2Category.Select(c => c.Cat.CategoryName)
                            .ToArray()
                        })
                        .ToArray()
                    })
                .SingleOrDefault();

            return posts;
        }

        //public VeganSavedVM DisplaySavedRecipes()
        //{
        //    string userId = userManager.GetUserId(accessor.HttpContext.User);

        //    var posts = context.SavedRecipe
        //        .Where(u => u.UserId == userId)
        //            .Select(u => new VeganSavedVM
        //            {
        //                Username = u.UserName,
        //                ProfileImg = u.PictureUrl,
        //                Posts = u.Recipe.Select(r => new PostItemVM3
        //                {
        //                    RecipeTitle = r.Title,
        //                    RecipeImg = r.Img,
        //                    RecipeCategories = r.Recipe2Category.Select(c => c.Cat.CategoryName)
        //                    .ToArray()
        //                })
        //                .ToArray()
        //            })
        //        .SingleOrDefault();

        //    return posts;
        //}

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

        internal void SaveImgToDB(IFormFile file, int id)
        {
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", fileName);
            using (var fileSrteam = new FileStream(filePath, FileMode.Create))
            {
                file.CopyToAsync(fileSrteam);
            }

            //var temp = new Recipe
            //{
            //    Img = filePath
            //};

            //context.Recipe.Add(temp);
            //context.SaveChanges();
            //var id = temp.Id;

            //return id;
        }

        internal int SetCategories(int[] array)
        {
            var temp = new Recipe();
            context.Recipe.Add(temp);
            context.SaveChanges();
            var id = temp.Id;

        

        
            for (int i = 0; i < array.Length; i++)
            {
                context.Recipe2Category.Add(new Recipe2Category
                {
                    CatId = array[i],
                    RecId = id
                   
                });
            }

            context.SaveChanges();

            return id;
        }
    }
}
