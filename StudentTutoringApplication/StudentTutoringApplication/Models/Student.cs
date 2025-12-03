using System;
using System.Collections.Generic;

namespace StudentTutoringApplication.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string UserId { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public string? Grade { get; set; }

    public string? Gpa { get; set; }

    public string? ResidencyStatus { get; set; }

    public string SubjectId { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Course Course { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
