using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentTutoringApplication.Models;

namespace StudentTutoringApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins {get; set;}
        public virtual DbSet<Appointment> Appointments {get; set;}
        public virtual DbSet<Schedule> Schedules {get; set;}
        public virtual DbSet<Student> Students {get; set;}
        public virtual DbSet<Tutor> Tutors {get; set;}
    }
}
