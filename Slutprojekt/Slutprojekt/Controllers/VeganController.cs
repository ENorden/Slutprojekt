using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Slutprojekt.Models;
using Slutprojekt.Models.ViewModels;

namespace Slutprojekt.Controllers
{
    [Authorize]
    public class VeganController : Controller
    {
        VeganService service;

        public VeganController(VeganService service)
        {
            this.service = service;
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
        [Route("")]
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
                return RedirectToAction(nameof(Register));
            else
                return Redirect(viewModel.ReturnUrl);
        }

        [HttpGet]
        [Route("recipes")]
        public IActionResult Recipes()
        {
            return View(service.GetAllCategories());
        }

        [HttpGet]
        [Route("followers")]
        public IActionResult Followers(VeganFollowersVM followersVM)
        {
            return View();
        }
    }
}