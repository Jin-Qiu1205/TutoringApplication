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
        public IActionResult Index()
        {
            // Provide an empty Tutor model to the view for form binding.
            return View(new TutorModel());
        }

        // POST: /Tutor/Home/SubmitAvailability
        // Step 1: Validate the data first. If successful, navigate to the AvailableSchedule confirmation page (do NOT save to the database yet).
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitAvailability(TutorModel tutor)
        {
            if (tutor == null)
            {
                return BadRequest("Tutor data is required.");
            }

            // ① Run annotation-based validation (such as [Required], etc.).
            if (!ModelState.IsValid)
            {
                return View("Index", tutor);
            }

            // ② Additional custom rule: The date must be after today and within the next 6 months.
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

            // ③ If any errors exist, return to the form page and display error messages under the corresponding fields.
            if (!ModelState.IsValid)
            {
                return View("Index", tutor);
            }

            // ④ All validations passed — do not save yet. Redirect to the confirmation page.
            return View("AvailableSchedule", tutor);
        }

        // POST: /Tutor/Home/ConfirmAvailability
        // Step 2: When clicking "Confirm & Save" on the AvailableSchedule page, execute this method and store the data into the database.
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
                // 1. Get the currently logged-in user.
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return Unauthorized();
                }
                tutor.UserId = currentUser.Id;

                // 2. SubjectId: Find or create a Subject.
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
                        SubjectId = tutor.Subject!, // Use the subject name as ID
                        SubjectName = tutor.Subject,
                        SubjectCode = tutor.Subject!.Substring(0, Math.Min(3, tutor.Subject.Length)).ToUpper(),
                        CourseId = defaultCourse.CourseId
                    };
                    _context.Subjects.Add(subject);
                    await _context.SaveChangesAsync();
                }
                tutor.SubjectId = subject.SubjectId;

                // 3. Create a Schedule entry.
                var schedule = new Schedule
                {
                    AvailabilityDay = DateOnly.FromDateTime(tutor.AvailableDate!.Value),
                    AvailabilityTime = tutor.AvailableTime,
                    Available = "Yes"
                };
                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();
                tutor.ScheduleId = schedule.ScheduleId;

                // 4. Save the Tutor record.
                await _context.Tutors.AddAsync(tutor);
                await _context.SaveChangesAsync();

                // 5. Display the confirmation page again (this time data is already saved).
                return View("AvailableSchedule", tutor);

                // If preferred, you may redirect back to form upon success:
                // return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Display a user-friendly message and keep the entered data.
                ModelState.AddModelError("", $"Error when saving: {ex.Message}");
                return View("AvailableSchedule", tutor);
            }
        }
    }
}