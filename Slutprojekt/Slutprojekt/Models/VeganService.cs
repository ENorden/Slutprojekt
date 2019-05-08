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
                new VeganIdentityUser { UserName = viewModel.Username, Email = viewModel.Email, FirstName = viewModel.FirstName, LastName = viewModel.LastName, PictureUrl = "profile-default.png" },
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

        internal VeganFollowersVM[] GetAllFollowers()
        {
            // Get current user's id
            string userId = userManager.GetUserId(accessor.HttpContext.User);

            var followingPosts = context.Follower
                .Where(u => u.FollowerId == userId)
                    .SelectMany(f => f.User.Recipe
                        .Select(r => new VeganFollowersVM
                        {
                            CreatorUsername = r.User.UserName,
                            CreatorProfileImg = r.User.PictureUrl,
                            RecipeTitle = r.Title,
                            RecipeImg = r.Img,
                            RecipeId = r.Id,
                            RecipeCategories = r.Recipe2Category.Select(c => c.Cat.CategoryName)
                                .ToArray()
                        }))
                        .OrderByDescending(r => r.RecipeId)
                        .ToArray();

            return followingPosts;

        }

        public async Task<VeganEditProfileVM> GetUserAsync()
        {
            // Hämta den inloggade användarens ID (från auth-cookie)
            string userId = userManager.GetUserId(accessor.HttpContext.User);

            // Hämta en användare baserat på ID:
            VeganIdentityUser user = await userManager.FindByIdAsync(userId);


            VeganEditProfileVM userVM = new VeganEditProfileVM()
            {
                UserName = user.UserName,
                Description = user.Description,               

            };

            return userVM;
        }

        public async Task UpdateUserProfileAsync(VeganEditProfileVM viewModel)
        {
            // Hämta den inloggade användarens ID (från auth-cookie)
            string userId = userManager.GetUserId(accessor.HttpContext.User);

            // Hämta en användare baserat på ID:
            VeganIdentityUser user = await userManager.FindByIdAsync(userId);


            // Hämta bild
            var fileName = Path.GetFileName(viewModel.PictureURL.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await viewModel.PictureURL.CopyToAsync(fileStream);
            }

            // Uppdatera en befintlig användare:
            user.UserName = viewModel.UserName;
            user.Description = viewModel.Description;
            user.PictureUrl = viewModel.PictureURL.FileName;

            await userManager.UpdateAsync(user);

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
                    UserId = r.Rec.UserId,
                    UserImg = r.Rec.User.PictureUrl,
                    Username = r.Rec.User.UserName,
                    CategoryName = r.Rec.Recipe2Category.Select(rc => rc.Cat.CategoryName)
                        .ToArray()
                }))
                .ToArray();

            return recipes;
        }

        public VeganDetailsVM GetRecipesById(int id)
        {
            // Get current users id
            string userId = userManager.GetUserId(accessor.HttpContext.User);

            // Find out if current recipe is saved by the current user
            bool isSaved = context.SavedRecipe
                .Any(r => r.RecId == id
                    && r.UserId == userId);

            // Find out if current recipe is created by the current user
            bool isUsersRecipe = context.Recipe
                .Any(r => r.Id == id
                    && r.UserId == userId);

            // Get user that created current recipe
            string creatorUserId = context.Recipe
                .First(r => r.Id == id).UserId;

            // Find if the recipe creator is followed by the current user
            bool isFollowing = context.Follower
                .Any(u => u.FollowerId == userId && u.UserId == creatorUserId);

            var recipe = context.Recipe
                .Where(r => r.Id == id)
                .Select(r => new VeganDetailsVM
                {
                    Title = r.Title,
                    RecImg = r.Img,
                    RecId = r.Id,
                    IsSaved = isSaved,
                    Categories = r.Recipe2Category.Select(c => c.Cat.CategoryName)
                        .ToArray(),
                    Username = r.User.UserName,
                    UserImg = r.User.PictureUrl,
                    UserId = r.User.Id,
                    Ingredients = r.Ingredient.Select(i => new IngredientVM
                    {
                        Name = i.Name,
                        Amount = i.Amount,
                        Unit = i.Unit
                    }).ToArray(),
                    Steps = r.StepByStep.Select(s => new StepByStepVM
                    {
                        Instruction = s.Instruction,
                        SortOrder = s.SortOrder
                    }).OrderBy(s => s.SortOrder).ToArray(),
                    IsFollowing = isFollowing,
                    IsUsersRecipe = isUsersRecipe
                })
                .SingleOrDefault();

            return recipe;
        }

        public string UnsaveRecipe(int id)
        {
            string userId = userManager.GetUserId(accessor.HttpContext.User);

            var savedRecipe = context.SavedRecipe
                .Where(u => u.RecId == id && u.UserId == userId)
                .Single();

            context.SavedRecipe.Remove(savedRecipe);
            context.SaveChanges();
                
            return "Save";
        }

        public string SaveRecipe(int id)
        {
            string userId = userManager.GetUserId(accessor.HttpContext.User);

            context.SavedRecipe.Add(new SavedRecipe
            {
                RecId = id,
                UserId = userId
            });
            context.SaveChanges();

            return "Unsave";
        }

        public string UnfollowPerson(string id)
        {
            string userId = userManager.GetUserId(accessor.HttpContext.User);

            var followedPerson = context.Follower
                .Where(u => u.UserId == id && u.FollowerId == userId)
                .Single();

            context.Follower.Remove(followedPerson);
            context.SaveChanges();

            return "Follow";
        }

        public string FollowPerson(string id)
        {
            string userId = userManager.GetUserId(accessor.HttpContext.User);

            context.Follower.Add(new Follower
            {
                UserId = id,
                FollowerId = userId
            });
            context.SaveChanges();

            return "Unfollow";
        }

        public VeganPostVM[] DisplayPosts()
        {
            string userId = userManager.GetUserId(accessor.HttpContext.User);

            var posts = context.Recipe
                .Where(r => r.UserId == userId)
                .Select(r => new VeganPostVM
                {
                    Username = r.User.UserName,
                    UserImg = r.User.PictureUrl,
                    RecipeTitle = r.Title,
                    RecipeImg = r.Img,
                    RecipeId = r.Id,
                    RecipeCategories = r.Recipe2Category.Select(c => c.Cat.CategoryName)
                            .ToArray()
                })
                .ToArray();

            return posts;
        }

        public VeganSavedVM[] DisplaySavedRecipes()
        {
            string userId = userManager.GetUserId(accessor.HttpContext.User);
            
            var posts = context.SavedRecipe
                .Where(u => u.UserId == userId)
                .Select(r => new VeganSavedVM
                {
                    CreatorUsername = r.Rec.User.UserName,
                    CreatorProfileImg = r.Rec.User.PictureUrl,
                    RecipeTitle = r.Rec.Title,
                    RecipeImg = r.Rec.Img,
                    RecipeId = r.RecId,
                    RecipeCategories = r.Rec.Recipe2Category.Select(c => c.Cat.CategoryName)
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
                    new SelectListItem { Value = "1", Text = "dl" },
                    new SelectListItem { Value = "2", Text = "msk", Selected = true },
                    new SelectListItem { Value = "3", Text = "tsk" },
                    new SelectListItem { Value = "4", Text = "g" },
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
                recipe.Title = viewModel.Title;
                recipe.Img = "temp";
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
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                viewModel.File.CopyToAsync(fileStream);
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

        public int AddRecipieStep2(StepTwo secondStep)
        {

            Ingredient ingredient = new Ingredient();
            ingredient.RecId = secondStep.RecID;
            ingredient.Name = secondStep.RecipeIngr;
            ingredient.Unit = secondStep.DropDownVal;
            ingredient.Amount = secondStep.RecipeAmount;
            context.Ingredient.Add(ingredient);

            context.SaveChanges();

            return ingredient.Id;

        }

        public void DeleteIngredient(DeleteIng delete)
        {
            Ingredient ingredient;

            ingredient = context.Ingredient.Find(delete.CurrentIngID);
            context.Ingredient.Remove(ingredient);
            context.SaveChanges();


        }

        public void AddRecipieStep3(Textbox description)
        {
            StepByStep stepByStep = new StepByStep();
            stepByStep.Instruction = description.TextBox;
            stepByStep.RecId = description.RecID;
            context.StepByStep.Add(stepByStep);
            context.SaveChanges();
        }
    }
}
