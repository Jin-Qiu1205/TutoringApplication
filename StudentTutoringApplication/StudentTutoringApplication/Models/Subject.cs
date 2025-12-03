using System;
using System.Collections.Generic;

namespace StudentTutoringApplication.Models;

public partial class Subject
{
    public string SubjectId { get; set; } = null!;

    public string SubjectCode { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Tutor> Tutors { get; set; } = new List<Tutor>();
}
