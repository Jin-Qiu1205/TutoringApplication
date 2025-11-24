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
        public DbSet<Tutor> Tutor { get; set; } = default!;
        public DbSet<Appointment> Appointment { get; set; } = default!;
        public DbSet<Student> Student { get; set; } = default!;
    }
}
