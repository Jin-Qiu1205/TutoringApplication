using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentTutoringApplication.Models;
using TutorModel = StudentTutoringApplication.Models.Tutor;
// using StudentTutoringApplication.Data;   

namespace StudentTutoringApplication.Areas.Tutor.Controllers
{
    [Area("Tutor")]
    [Authorize(Roles = "Tutor")]
    public class HomeController : Controller
    {
        // for connect DB
        // private readonly ApplicationDbContext _context;
    
        // public HomeController(ApplicationDbContext context)
        // {
        //     _context = context;
        // }


      

     
        public IActionResult Index()
        {
            CreateDropdowns();
            var model = new TutorModel();
            return View(model);
        }

        // GET: /Tutor/Home/CreateTutorDebug
        [HttpGet]
        public IActionResult CreateTutorDebug()
        {
            CreateDropdowns();              
            var model = new TutorModel();        
            return View(model);
        }

        // POST: /Tutor/Home/CreateTutorDebug
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(TutorModel tutor)
        {

            ModelState.Remove("Email");
            ModelState.Remove("Password");
            if (!ModelState.IsValid)
            {
                
                CreateDropdowns();
                return View(tutor);
            }


            // connect to datebase,for future
            // _context.Tutor.Add(tutor);
            // _context.SaveChanges();


            return View("AvailableSchedule", tutor);
        }

 
        private void CreateDropdowns()
        {
     
            ViewBag.SubjectList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Introduction to Source Control", Value = "Introduction to Source Control" },
                new SelectListItem { Text = "Responsive Web Design",         Value = "Responsive Web Design" },
                new SelectListItem { Text = "PHP",                           Value = "PHP" },
                new SelectListItem { Text = "Enterprise Java",               Value = "Enterprise Java" },
                new SelectListItem { Text = "MVC Framework",                 Value = "MVC Framework" }
            };

   
            var timeSlots = new List<SelectListItem>();

            for (int h = 8; h < 20; h += 2)   // 8,10,12,14,16,18
            {
                var start = new TimeSpan(h, 0, 0);
                var end = new TimeSpan(h + 2, 0, 0);

                string display =
                    $"{DateTime.Today.Add(start):hh\\:mm tt} - {DateTime.Today.Add(end):hh\\:mm tt}";

                timeSlots.Add(new SelectListItem
                {
                    Text = display,
                    Value = display
                });
            }

            ViewBag.TimeSlots = timeSlots;
        }
    }
}
