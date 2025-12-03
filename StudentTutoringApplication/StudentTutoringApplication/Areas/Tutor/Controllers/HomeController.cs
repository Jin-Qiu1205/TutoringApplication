using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTutoringApplication.Models;

// Give the Tutor model an alias to avoid a conflict with the namespace Areas.Tutor.
using TutorModel = StudentTutoringApplication.Models.Tutor;

namespace StudentTutoringApplication.Areas.Tutor.Controllers
{
    [Area("Tutor")]
    [Authorize(Roles = "Tutor")]
    public class HomeController : Controller
    {
        // Use the TutoringContext we scaffolded (which is connected to the TutoringApplication database).
        private readonly TutoringContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(TutoringContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Tutor/Home/Index
        // Show the availability form.
        public IActionResult Index()
        {
       
            return View(new TutorModel());
        }

        // GET: /Tutor/Home/List
     
        public async Task<IActionResult> List()
        {
            var tutors = await _context.Tutors
                .Where(t => t.Appointments.Any())
                .Include(t => t.Appointments)
                .AsNoTracking()
                .ToListAsync();

            return View(tutors);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAvailability(TutorModel tutor)
        {
            if (tutor == null)
            {
                return BadRequest("Tutor data is required.");
            }

           
            if (!ModelState.IsValid)
            {
               
                return View("Index", tutor);
            }

            // 2) Additional custom rule: The date must be after today and within the next 6 months.
            if (tutor.AvailableDate.HasValue)
            {
                var today = DateTime.Today;
                var sixMonthsLater = today.AddMonths(6);

                if (tutor.AvailableDate.Value <= today)
                {
                    ModelState.AddModelError(
                        nameof(TutorModel.AvailableDate),
                        "Available date must be after today.");
                }
                else if (tutor.AvailableDate.Value > sixMonthsLater)
                {
                    ModelState.AddModelError(
                        nameof(TutorModel.AvailableDate),
                        "Available date must be within the next 6 months.");
                }
            }

            // 2.5) Custom validation: same tutor, same date, same time is not allowed.

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            tutor.UserId = currentUser.Id;

            if (tutor.AvailableDate.HasValue && !string.IsNullOrWhiteSpace(tutor.AvailableTime))
            {
                bool timeSlotExists = await _context.Tutors.AnyAsync(t =>
                    t.UserId == tutor.UserId &&
                    t.AvailableDate == tutor.AvailableDate &&
                    t.AvailableTime == tutor.AvailableTime);

                if (timeSlotExists)
                {
                    // This error will show on the Index page (Tutor Availability).
                    ModelState.AddModelError(string.Empty, "This time slot is not available.");
                }
            }

            // 3) If any errors exist (date rule or time-slot rule),
            // return to the form page and display error messages on Index.cshtml.
            if (!ModelState.IsValid)
            {
                return View("Index", tutor);
            }

            // 4) All validations passed — do not save yet. Redirect to the confirmation page.
            return View("AvailableSchedule", tutor);
        }

       
        // Step 2: When clicking "Confirm & Save" on the AvailableSchedule page,
        // execute this method and store the data into the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmAvailability(TutorModel tutor)
        {
            if (tutor == null)
            {
                return BadRequest("Tutor data is required.");
            }

            // Perform a quick basic validation check again to avoid saving empty/invalid data.
            if (!ModelState.IsValid)
            {
                return View("AvailableSchedule", tutor);
            }

            try
            {
                // 1) Get the currently logged-in user.
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return Unauthorized();
                }
                tutor.UserId = currentUser.Id;

             

                // 2) SubjectId: Find or create a Subject.
                var subject = await _context.Subjects
                    .FirstOrDefaultAsync(s => s.SubjectName == tutor.Subject);

                if (subject == null)
                {
                    // Find or create a default Course if needed.
                    var defaultCourse = await _context.Courses
                        .FirstOrDefaultAsync(c => c.CourseId == "DEFAULT");

                    if (defaultCourse == null)
                    {
                        defaultCourse = new Course
                        {
                            CourseId = "DEFAULT",
                            CourseCode = "DEF",
                            Title = "Default Course",
                            Prerequisite = "None"
                        };
                        _context.Courses.Add(defaultCourse);
                        await _context.SaveChangesAsync();
                    }

                    subject = new Subject
                    {
                        // Use the subject name as ID for simplicity.
                        SubjectId = tutor.Subject!,
                        SubjectName = tutor.Subject,
                        SubjectCode = tutor.Subject!.Substring(0, Math.Min(3, tutor.Subject.Length)).ToUpper(),
                        CourseId = defaultCourse.CourseId
                    };

                    _context.Subjects.Add(subject);
                    await _context.SaveChangesAsync();
                }
                tutor.SubjectId = subject.SubjectId;

                // 3) Create a Schedule entry.
                var schedule = new Schedule
                {
                    AvailabilityDay = DateOnly.FromDateTime(tutor.AvailableDate!.Value),
                    AvailabilityTime = tutor.AvailableTime,
                    Available = "Yes"
                };
                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();
                tutor.ScheduleId = schedule.ScheduleId;

                // 4) Save the Tutor record.
                await _context.Tutors.AddAsync(tutor);
                await _context.SaveChangesAsync();
                ViewBag.SuccessMessage = "Your availability has been saved successfully.";
                // 5) Display the confirmation page again (this time data is already saved).
                return View("AvailableSchedule", tutor);

                // If preferred, you may redirect back to the form upon success:
                // return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Display a user-friendly message and keep the entered data.
                ModelState.AddModelError(string.Empty, $"Error when saving: {ex.Message}");
                return View("AvailableSchedule", tutor);
            }
        }
    }
}
