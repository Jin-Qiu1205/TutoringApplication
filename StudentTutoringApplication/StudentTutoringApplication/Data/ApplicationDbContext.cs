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
        public DbSet<StudentTutoringApplication.Models.Tutor> Tutor { get; set; } = default!;
        public DbSet<StudentTutoringApplication.Models.Appointment> Appointment { get; set; } = default!;
        public DbSet<StudentTutoringApplication.Models.Student> Student { get; set; } = default!;
    }
}
