using System;
using System.Collections.Generic;

namespace StudentTutoringApplication.Models;

public partial class Tutor
{
    public int TutorId { get; set; }

    public string SubjectId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public int ScheduleId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
