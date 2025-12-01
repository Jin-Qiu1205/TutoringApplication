using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StudentTutoringApplication.Models;

public partial class Course
{
    [Key]
    [BindNever]
    public string CourseId { get; set; }

    public string CourseCode { get; set; }

    public string? Credits { get; set; } = string.Empty;

    public string? CourseLength { get; set; } = string.Empty;

    public string? Classroom { get; set; } = string.Empty;

    public string? Title { get; set; } = string.Empty;

    public string? Prerequisite { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    // Dropdown values for Prerequisites.
    public enum PrerequisiteType
    {
        [Display(Name = "None")]
        None
    };

    public enum ClassroomType
    {
        [Display(Name = "A1010")]
        A1010,
        [Display(Name = "A1011")]
        A1011
    }

    public Course() { }
    public Course(string courseCode, string credits, string courseLength, string classroom, string title, string prerequisite)
    {
        this.CourseCode = courseCode;
        this.Credits = credits;
        this.CourseLength = courseLength;
        this.Classroom = classroom;
        this.Title = title;

    }
}
