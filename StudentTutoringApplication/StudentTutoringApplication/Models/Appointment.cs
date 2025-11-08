using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTutoringApplication.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public int TutorId { get; set; }
        public int StudentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Status { get; set; } = "Scheduled"; // e.g., Scheduled, Completed, Canceled
        public string StatusMessage { get; set; }

        // FK's

        //Navigation properties:

        public Appointment() { }
        /*
        public Appointment()
        {
            // Needs something other than just Ids as we shouldn't manually set Id's.
        }
        */
        
    }
}
