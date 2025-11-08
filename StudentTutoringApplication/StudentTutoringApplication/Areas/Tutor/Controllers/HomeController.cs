using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentTutoringApplication.Areas.Tutor.Controllers
{
    [Area("Tutor")]
    [Authorize(Roles = "Tutor")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
