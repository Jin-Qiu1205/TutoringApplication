using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTutoringApplication.Models
{
    public class Tutor
    {
        [Key]
        public int TutorId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        public string Availability {  get; set; } = string.Empty; // PLACEHOLDER
        public string[] Subjects { get; set; } // PLACEHOLDER

        // FK's


        // Navigation Properties

        public Tutor() { }
        public Tutor(string name, string email, string password)
        {
            this.Name = name;
            this.Email = email;
            this.Password = password;
        }
    }
}
