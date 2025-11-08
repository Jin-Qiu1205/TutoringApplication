using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTutoringApplication.Data;
using StudentTutoringApplication.Models;
using System.Diagnostics.Metrics;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace StudentTutoringApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
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

        // Role Index page Get method
        [HttpGet]
        public async Task<IActionResult> Index(LoginViewModel model)
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

                if (roles.Contains("Student")){

                    var tutor = from i in _context.Tutor select i;
                    //var newModel = await _context.Tutor.ToListAsync(); // Placeholder unless there isnt a better way to do it
                    return View(await PaginatedList<Tutor>.CreateAsync(tutor.AsNoTracking(), 1, 10));
                }

                // fallback redirect if user has no role
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Invalid login attempt.";
            return View(model);
        }

        // GET: BookAppointment
        [HttpGet]
        public async Task<IActionResult> BookAppointment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutor = await _context.Tutor
                .Where(x => x.TutorId == id)
                .SingleOrDefaultAsync();

            if (tutor == null)
            {
                return NotFound();
            }


            return View(tutor);
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
