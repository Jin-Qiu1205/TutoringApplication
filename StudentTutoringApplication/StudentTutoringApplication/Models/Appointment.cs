using System;
using System.Collections.Generic;

namespace StudentTutoringApplication.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int TutorId { get; set; }

    public int StudentId { get; set; }

    public int ScheduleId { get; set; }

    public string SubjectId { get; set; } = null!;

    public string? Status { get; set; }

    public string? Rating { get; set; }

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;

    public virtual Tutor Tutor { get; set; } = null!;
}
