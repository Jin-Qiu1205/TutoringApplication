using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTutoringApplication.Models;

// "Give the Tutor model an alias to avoid conflicts with the Areas.Tutor namespace
using TutorModel = StudentTutoringApplication.Models.Tutor;

namespace StudentTutoringApplication.Areas.Tutor.Controllers
{
    [Area("Tutor")]
    [Authorize(Roles = "Tutor")]
    public class HomeController : Controller
    {
        //Use TutoringContext (located in the Models namespace)
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

            return View(new TutorModel());
        }

        // POST: /Tutor/Home/SubmitAvailability
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAvailability(TutorModel tutor)
        {
            if (tutor == null)
            {
                return BadRequest("Tutor data is required.");
            }

            // 让 DataAnnotations 先跑一遍（Required 等）
            // 这里不用 ModelState.Clear() 了
            // 只加额外的日期范围验证逻辑

            if (tutor.AvailableDate.HasValue)
            {
                var today = DateTime.Today;
                var sixMonthsLater = today.AddMonths(6);

                if (tutor.AvailableDate.Value <= today)
                {
                    ModelState.AddModelError(nameof(TutorModel.AvailableDate),
                        "Available date must be after today.");
                }
                else if (tutor.AvailableDate.Value > sixMonthsLater)
                {
                    ModelState.AddModelError(nameof(TutorModel.AvailableDate),
                        "Available date must be within the next 6 months.");
                }
            }

            // 任何验证错误 → 回到同一页面，显示错误
            if (!ModelState.IsValid)
            {
                return View("Index", tutor);
            }

            try
            {
                // 1. 当前登录用户
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return Unauthorized();
                }
                tutor.UserId = currentUser.Id;

                // 2. SubjectId - 查找或创建 Subject
                var subject = await _context.Subjects
                    .FirstOrDefaultAsync(s => s.SubjectName == tutor.Subject);

                if (subject == null)
                {
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
                        SubjectId = tutor.Subject!,
                        SubjectName = tutor.Subject,
                        SubjectCode = tutor.Subject!.Substring(0, Math.Min(3, tutor.Subject.Length)).ToUpper(),
                        CourseId = defaultCourse.CourseId
                    };
                    _context.Subjects.Add(subject);
                    await _context.SaveChangesAsync();
                }
                tutor.SubjectId = subject.SubjectId;

                // 3. 创建 Schedule
                var schedule = new Schedule
                {
                    AvailabilityDay = DateOnly.FromDateTime(tutor.AvailableDate!.Value),
                    AvailabilityTime = tutor.AvailableTime,   // 现在是 nvarchar(50)，可以直接存
                    Available = "Yes"
                };
                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();
                tutor.ScheduleId = schedule.ScheduleId;

                // 4. 保存 Tutor
                await _context.Tutors.AddAsync(tutor);
                await _context.SaveChangesAsync();

                // 5. 确保 Subject 字符串有值
                if (string.IsNullOrEmpty(tutor.Subject))
                {
                    tutor.Subject = subject.SubjectName;
                }

                // 6. 成功 → 跳到 AvailableSchedule 视图
                return View("AvailableSchedule", tutor);
            }
            catch (Exception ex)
            {
                // 调试期先看清错误
                var msg = ex.InnerException?.Message ?? ex.Message;
                ModelState.AddModelError("", $"Error when saving: {msg}");
                return View("Index", tutor);
            }
        }
    }
}
