using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreProject.Domain.Identity;
using DotNetCoreProject.Web.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreProject.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //[Route("Account/Login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
       // [Route("Account/Login")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var existUser = await _userManager.FindByEmailAsync(model.UserName);
                if (existUser == null)
                {
                    ModelState.AddModelError(string.Empty, "Böyle Bir Kullanıcı Bulunamadı");
                    return View(model);
                }
                var login = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (login == null)
                {
                    ModelState.AddModelError(string.Empty, "Bu Email ya da Şifre ile Uyumlu Bir Kullanıcı Bulunamadı");
                    return View(model);
                }
                if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        [Route("Account/Register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Route("Account/Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    // NationalIdNumber = model.NationalIdNumber,
                    Email = model.Email,
                    EmailConfirmed = true,
                    UserName = model.Email,
                    TwoFactorEnabled = false

                };
                var registerUser = await _userManager.CreateAsync(newUser, model.Password);
                if (registerUser.Succeeded)
                {
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(registerUser);
            }
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }
       
    }
}