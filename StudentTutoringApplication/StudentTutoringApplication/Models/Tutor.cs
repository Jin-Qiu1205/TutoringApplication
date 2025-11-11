using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace StudentTutoringApplication.Models
{
    public class Tutor
    {
        [Key]
        public int TutorId { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
<<<<<<< Updated upstream
=======
        [Required]
        public string Password { get; set; } = string.Empty;

        public string Subject { get; set; } // PLACEHOLDER

        public DateTime AvailableDate { get; set; }  
        public string AvailableTime { get; set; }
>>>>>>> Stashed changes

        // FK's
        [NotMapped] 
        public string Name
        {
            get => $"{FirstName} {LastName}".Trim();
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    FirstName = string.Empty;
                    LastName = string.Empty;
                    return;
                }

                var parts = value.Trim()
                                 .Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                FirstName = parts[0];
                LastName = parts.Length > 1 ? parts[1] : string.Empty;
            }
        }

      
        [NotMapped]
        public string Availability
        {
            get
            {
                if (AvailableDate == default)
                    return string.Empty;

                if (string.IsNullOrWhiteSpace(AvailableTime))
                    return AvailableDate.ToString("yyyy-MM-dd");

                return $"{AvailableDate:yyyy-MM-dd} {AvailableTime}";
            }
            set
            {
                
            }
        }

   
        [NotMapped]
        public string[] Subjects
        {
            get => string.IsNullOrWhiteSpace(Subject)
                ? Array.Empty<string>()
                : new[] { Subject };
            set
            {
                if (value == null || value.Length == 0)
                {
                    Subject = string.Empty;
                }
                else
                {
              
                    Subject = value[0];
                }
            }
        }

        // Navigation Properties
        public Tutor() { }
<<<<<<< Updated upstream
        public Tutor(string name, string email)
=======
        public Tutor(string firstName, string lastName, string email, string password,
                    string subject, DateTime availableDate, string availableTime)
>>>>>>> Stashed changes
        {
           
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
<<<<<<< Updated upstream
=======
            this.Password = password;
            this.Subject = subject;
            this.AvailableDate = availableDate;
            this.AvailableTime = availableTime;
>>>>>>> Stashed changes
        }
    }
}

