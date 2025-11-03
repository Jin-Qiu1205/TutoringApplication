using System.ComponentModel.DataAnnotations;

namespace StudentTutoringApplication.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        // FK's:


        // Navigation Properties:

        public Admin() { }

        public Admin(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

    }
}
