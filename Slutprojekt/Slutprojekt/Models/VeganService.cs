using Microsoft.AspNetCore.Identity;
using Slutprojekt.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slutprojekt.Models
{
    public class VeganService
    {
        UserManager<VeganIdentityUser> userManager;
        SignInManager<VeganIdentityUser> signInManager;

        public VeganService(
            UserManager<VeganIdentityUser> userManager,
            SignInManager<VeganIdentityUser> signInManager
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        readonly VeganIdentityContext context;

        public VeganService(VeganIdentityContext context)
        {
            this.context = context;
        }

        public async Task<IdentityResult> TryRegisterAsync(VeganRegisterVM viewModel)
        {
            // Try to create a new user
            return await userManager.CreateAsync(
                new VeganIdentityUser { UserName = viewModel.Username, Email = viewModel.Email, FirstName =viewModel.FirstName, LastName = viewModel.LastName },
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

        

        internal VeganFollowersVM[] GetAllFollowers(VeganFollowersVM followersVM)
        {
            return context.Users
                .Select(person => new VeganFollowersVM
                {
                    Username = person.UserName,
                    FirstName = person.FirstName,
                    Posts = { "Hello", "My recipe" }
                })
                .ToArray();
        }

            public VeganRecipeVM GetAllCategories()
            {
            VeganRecipeVM test = new VeganRecipeVM();
            return test;
        }
    }
}
