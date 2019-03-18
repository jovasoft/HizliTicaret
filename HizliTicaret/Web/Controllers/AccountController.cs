using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private RoleManager<Role> roleManager;
        private SignInManager<User> signInManager;

        public AccountController(UserManager<User> _userManager, RoleManager<Role> _roleManager, SignInManager<User> _signInManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            signInManager = _signInManager;

        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Mail
                };

                IdentityResult userResult = userManager.CreateAsync(user, registerViewModel.Password).Result;

                if (userResult.Succeeded)
                {
                    Role role = new Role { Name = "Admin" };

                    if (!roleManager.RoleExistsAsync(role.Name).Result)
                    {
                        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                    }

                    userManager.AddToRoleAsync(user, role.Name);

                    return RedirectToAction("Login");
                }

                ModelState.AddModelError("registerError", "Hesap oluşturulamadı.");
            }

            return View(registerViewModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, loginViewModel.RememberMe, false).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("loginError", "Hatalı kullanıcı adı veya parola.");
            }

            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            signInManager.SignOutAsync().Wait();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}