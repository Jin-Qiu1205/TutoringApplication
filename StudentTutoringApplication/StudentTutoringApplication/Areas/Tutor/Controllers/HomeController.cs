using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTutoringApplication.Data;

namespace StudentTutoringApplication.Areas.Tutor.Controllers
{
    [Area("Tutor")]
    [Authorize(Roles = "Tutor")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context){
            
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateTutorDebug()
        {
            return View();
        }

        // POST: api/tutors
        [HttpPost]
        public async Task<IActionResult> CreateTutorDebug(StudentTutoringApplication.Models.Tutor tutor)
        {
            if (tutor == null)
            {
                return BadRequest("Tutor data is required.");
            }

            // Optional: basic model validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Add the new tutor to the database context
            await _context.Tutor.AddAsync(tutor);
            await _context.SaveChangesAsync();

            // Return a 201 Created response with a route to the new resource
            return RedirectToAction("Index", "Home", new { area = "Tutor" });
        }
    }
}
