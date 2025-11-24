using System;
using System.Collections.Generic;

namespace StudentTutoringApplication.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public DateOnly AvailabilityDay { get; set; }

    public TimeOnly? AvailabilityTime { get; set; }

    public string? Available { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Tutor> Tutors { get; set; } = new List<Tutor>();
}
