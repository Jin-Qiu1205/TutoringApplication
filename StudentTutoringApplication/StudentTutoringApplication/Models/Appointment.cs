using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTutoringApplication.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        // FK's
        [Required]
        public int TutorId { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int ScheduleId { get; set; }

        //Navigation properties:
        [ForeignKey("TutorId")]
        public Tutor Tutor { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }
        [ForeignKey("ScheduleId")]
        public Schedule Schedule { get; set; }

        public Appointment() { }
        /*
        public Appointment()
        {
            // Needs something other than just Ids as we shouldn't manually set Id's.
        }
        */
        
    }
}
