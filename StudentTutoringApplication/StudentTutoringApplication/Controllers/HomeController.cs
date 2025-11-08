using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentTutoringApplication.Models;

namespace StudentTutoringApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            if (User.IsInRole("Tutor"))
                return RedirectToAction("Index", "Home", new { area = "Tutor" });
            if (User.IsInRole("Student"))
                return RedirectToAction("Index", "Home", new { area = "Student" });

            return View(); // fallback
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
