using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTutoringApplication.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        // vvv Likely better as an enum in 15 or 30 minute blocks on during business days.
        [Required]
        public DateOnly AvailabilityDay { get; set; }
        [Required]
        public TimeOnly AvailabilityTime { get; set; }
        [Required]
        public bool Available { get; set; } = false;

        // FK's
        

        // Navigation Properties


        public Schedule() { }
        public Schedule(DateOnly availabilityDay, TimeOnly availabilityTime, bool available)
        {
            this.AvailabilityDay = availabilityDay;
            this.AvailabilityTime = availabilityTime;
            this.Available = available;
        }
    }
}
