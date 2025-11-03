using System.ComponentModel.DataAnnotations;

namespace StudentTutoringApplication.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public Student() { }
        public Student(string name, string email)
        {
            this.Name = name;
            this.Email = email;
        }
    }
}
