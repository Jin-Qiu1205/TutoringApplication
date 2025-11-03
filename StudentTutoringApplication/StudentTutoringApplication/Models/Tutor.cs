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

        // FK's


        // Navigation Properties

        public Tutor() { }
        public Tutor(string name, string email)
        {
            this.Name = name;
            this.Email = email;
        }
    }
}
