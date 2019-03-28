using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private RoleManager<Role> roleManager;
        private SignInManager<User> signInManager;
        private IFavoriteService favoriteService;
        private ISaleService saleService;

        public AccountController(UserManager<User> _userManager, RoleManager<Role> _roleManager, SignInManager<User> _signInManager, IFavoriteService _favoriteService, ISaleService _saleService)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            signInManager = _signInManager;
            favoriteService = _favoriteService;
            saleService = _saleService;
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User {
                    UserName = registerViewModel.Mail,
                    Email = registerViewModel.Mail,
                    Name = registerViewModel.Name
                };

                IdentityResult userResult = new IdentityResult();
                if(registerViewModel.Password == registerViewModel.ConfirmPassword)
                {
                    userResult = userManager.CreateAsync(user, registerViewModel.Password).Result;

                }
                else
                {
                    ModelState.AddModelError("", "Girilen şifreler uyuşmuyor.");
                }

                if (userResult.Succeeded)
                {
                    if (!roleManager.RoleExistsAsync("User").Result)
                    {
                        Role role = new Role { Name = "User" };
                        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                    }

                    userManager.AddToRoleAsync(user, "User").Wait();

                    return RedirectToAction("Login");
                }

                foreach (var item in userResult.Errors)
                {
                    if (item.Code == "DuplicateUserName" || item.Code == "InvalidUserName") continue;

                    ModelState.AddModelError("", item.Description);
                }
            }

            return View(registerViewModel);
        }

        public IActionResult Login()
        {
            bool test = User.IsInRole("User");

            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = signInManager.PasswordSignInAsync(loginViewModel.Mail, loginViewModel.Password, loginViewModel.RememberMe, false).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("loginError", "Hatalı kullanıcı adı veya parola.");
            }

            return View(loginViewModel);
        }

        public IActionResult Logout()
        {
            signInManager.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("SuccessfullyReset");
            }

            return View(forgotPasswordViewModel);
        }

        public IActionResult SuccessfullyReset()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        #region autherized

        [Authorize(Roles = "Admin, User, Merchant")]
        public IActionResult Orders()
        {
            return View();
        }

        [Authorize(Roles = "Admin, User, Merchant")]
        public IActionResult Manage()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User, Merchant")]
        [ValidateAntiForgeryToken]
        public IActionResult Manage(ManageViewModel manageViewModel)
        {
            if (ModelState.IsValid)
            {

            }

            return View(manageViewModel);
        }

        [Authorize(Roles = "Admin, User, Merchant")]
        public IActionResult Favorites()
        {
            List<Favorite> favorites = favoriteService.GetList(User.Identity.Name);

            return View(favorites);
        }

        [Authorize(Roles = "Admin, User, Merchant")]
        public IActionResult DeleteFavorites(Guid id)
        {
            if (id != Guid.Empty)
            {
                try
                {
                    favoriteService.Delete(id);
                }
                catch (Exception) { }
            }

            Response.StatusCode = 200;
            return Json( new { status = "success" });
        }

        #endregion
    }
}