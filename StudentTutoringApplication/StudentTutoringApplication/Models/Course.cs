using System;
using System.Collections.Generic;

namespace StudentTutoringApplication.Models;

public partial class Course
{
    public string CourseId { get; set; } = null!;

    public string CourseCode { get; set; } = null!;

    public string? Credits { get; set; }

    public string? CourseLength { get; set; }

    public string? Classroom { get; set; }

    public string? Title { get; set; }

    public string Prerequisite { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
