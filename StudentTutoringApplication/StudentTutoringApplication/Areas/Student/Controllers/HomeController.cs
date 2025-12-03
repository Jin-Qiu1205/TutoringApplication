using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using StudentTutoringApplication.Data;
using StudentTutoringApplication.Models;
using AppointmentModel = StudentTutoringApplication.Models.Appointment;
using TutorModel = StudentTutoringApplication.Models.Tutor;

namespace StudentTutoringApplication.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager) {
        
            _context = context;
            _userManager = userManager;
            // _context.
        }

        public async Task<IActionResult> Index(

        string tutorFirstName,
        string tutorLastName,
        string subject,
        string submitType)
        {
            // Always start with full Tutor table.
            // Use TutorModel because "Tutor" is also the name of the Area namespace.
            IQueryable<TutorModel> query = _context.Set<TutorModel>().AsQueryable();

            switch (submitType)
            {
                case "SearchTutor":
                    // Must enter BOTH first and last names
                    if (string.IsNullOrWhiteSpace(tutorFirstName) ||
                        string.IsNullOrWhiteSpace(tutorLastName))
                    {
                        ViewBag.ErrorMessage = "Please enter a tutor's first name and last name.";
                    }
                    else
                    {
                        string fn = tutorFirstName.Trim().ToLower();
                        string ln = tutorLastName.Trim().ToLower();

                        query = query.Where(t =>
                            t.FirstName.ToLower() == fn &&
                            t.LastName.ToLower() == ln);
                    }
                    break;

                case "SearchSubject":
                    if (string.IsNullOrWhiteSpace(subject))
                    {
                        ViewBag.ErrorMessage = "Please enter a subject name.";
                    }
                    else
                    {
                        string s = subject.Trim().ToLower();

                        // SubjectId is a string.
                        query = query.Where(t =>
                            t.SubjectId.ToLower().Contains(s));
                    }
                    break;

                case "AllTutors":
                case "AllSubjects":
                default:
                    // No filtering → Return ALL tutors
                    break;
            }

            var result = await query.ToListAsync();
            return View(result);
        }

        private async Task PopulateViewBag(AppointmentModel model)
        {
            var tutor = await _context.Tutor.FindAsync(model.TutorId);
            var student = await _context.Student.FindAsync(model.StudentId);
            var user = await _context.Users.FindAsync(student.UserId);

            ViewBag.TutorId = tutor.TutorId;
            ViewBag.StudentId = student.StudentId;
            ViewBag.StudentName = user.UserName;
            ViewBag.ScheduleId = model.ScheduleId;
            ViewBag.SubjectId = model.SubjectId;
            ViewBag.TutorName = tutor.FirstName + " " + tutor.LastName;
            ViewBag.Date = tutor.AvailableDate;
            ViewBag.Time = tutor.AvailableTime;
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

            var user = await _userManager.GetUserAsync(User);

            var student = await _context.Student
                .Where(x => x.UserId == user.Id)
                .SingleOrDefaultAsync();

            ViewBag.Tutorid = tutor.TutorId;
            ViewBag.StudentId = student.StudentId;
            ViewBag.StudentName = user.UserName;
            ViewBag.ScheduleId = tutor.ScheduleId;
            ViewBag.SubjectId = tutor.SubjectId;
            ViewBag.TutorName = tutor.FirstName + " " + tutor.LastName;

            ViewBag.Date = tutor.AvailableDate;
            ViewBag.Time = tutor.AvailableTime;

            var appointment = new Models.Appointment
            {
                TutorId = tutor.TutorId,
                StudentId = student.StudentId,
                ScheduleId = tutor.ScheduleId,
                SubjectId = tutor.SubjectId,
                Status = null,
                Rating = null
            };

            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment(AppointmentModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _context.Student.SingleAsync(x => x.UserId == user.Id);

            var tutor = await _context.Tutor.SingleAsync(x => x.TutorId == model.TutorId);

            model.AppointmentId = 0; // ensure a new record is created
            model.TutorId = tutor.TutorId; // force correct tutor
            model.StudentId = student.StudentId; // force correct student
            model.SubjectId = tutor.SubjectId;
            model.ScheduleId = tutor.ScheduleId;

            model.Status = null;
            model.Rating = null;

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                throw new InvalidOperationException("The appointment data is invalid.");
            }

            _context.Appointment.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
