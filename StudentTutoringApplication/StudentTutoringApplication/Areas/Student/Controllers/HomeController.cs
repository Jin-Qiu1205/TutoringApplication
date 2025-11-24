using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTutoringApplication.Data;

namespace StudentTutoringApplication.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context) {
        
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Tutor.ToListAsync());
        }

        // GET: BookAppointment
        [HttpGet]
        public async Task<IActionResult> BookAppointment(int? id)
        {
            // need to add a bunch of stuff to viewbag

            if (id == null){
                return NotFound();
            }

            var tutor = await _context.Tutor
                .Where(x => x.TutorId == id)
                .SingleOrDefaultAsync();

            if (tutor == null){
                return NotFound();
            }

            return View();
        }
    }
}
