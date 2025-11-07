using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTutoringApplication.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

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
