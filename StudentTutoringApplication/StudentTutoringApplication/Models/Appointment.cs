using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTutoringApplication.Models;

public partial class Appointment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AppointmentId { get; set; }

    public int TutorId { get; set; }

    public int StudentId { get; set; }

    public int? ScheduleId { get; set; }

    public string SubjectId { get; set; } = null!;

    public string? Status { get; set; } = null;

    public string? Rating { get; set; } = null;


    // Commented out for compatibility with appointment page in Student area
    //public virtual Schedule Schedule { get; set; } = null!;

    //public virtual Student Student { get; set; } = null!;

    //public virtual Subject Subject { get; set; } = null!;

    //public virtual Tutor Tutor { get; set; } = null!;
}
