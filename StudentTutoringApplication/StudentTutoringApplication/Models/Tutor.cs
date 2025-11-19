using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTutoringApplication.Models;

public partial class Tutor
{
    public int TutorId { get; set; }

    // These are database foreign key fields. They are not entered directly in the form,
    // so they are nullable to avoid automatic Required validation errors.
    public string? SubjectId { get; set; }
    public string? UserId { get; set; }
    public int? ScheduleId { get; set; }

    // ======== Fields entered in the form (used in Tutor/Index.cshtml) =========

    [Required(ErrorMessage = "First Name is required.")]
    public string? FirstName { get; set; }       //  Do NOT mark as NotMapped

    [Required(ErrorMessage = "Last Name is required.")]
    public string? LastName { get; set; }        //  Do NOT mark as NotMapped

    [Required(ErrorMessage = "Available date is required.")]
    [DataType(DataType.Date)]
    public DateTime? AvailableDate { get; set; } //  Do NOT mark as NotMapped

    [Required(ErrorMessage = "Available time is required.")]
    public string? AvailableTime { get; set; }   //  Do NOT mark as NotMapped

    // Used for dropdown binding (string value). Not mapped to the database.
    [NotMapped]
    [Required(ErrorMessage = "Subject is required.")]
    public string? Subject { get; set; }

    // ======== Extra fields added for compatibility with Student area views =========

    [NotMapped]
    public string? Name { get; set; }

    [NotMapped]
    public string? Subjects { get; set; }

    [NotMapped]
    public string? Availability { get; set; }

    // =============== Navigation properties (database relationships) ================

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Schedule? Schedule { get; set; }

    public virtual Subject? SubjectNavigation { get; set; }

    public virtual AspNetUser? User { get; set; }
}

