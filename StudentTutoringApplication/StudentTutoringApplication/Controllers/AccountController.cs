using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentTutoringApplication.Models;
using System.Threading.Tasks;

namespace StudentTutoringApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                if (roles.Contains("Tutor"))
                    return RedirectToAction("Index", "Home", new { area = "Tutor" });

                if (roles.Contains("Student"))
                    return RedirectToAction("Index", "Home", new { area = "Student" });

                // fallback redirect if user has no role
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Invalid login attempt.";
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


    }
}
