using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTutoringApplication.Models;

public partial class Tutor
{
    public int TutorId { get; set; }

    // 这些是数据库外键字段，在表单里不直接填写，所以设成可空，避免自动 Required 错误
    public string? SubjectId { get; set; }
    public string? UserId { get; set; }
    public int? ScheduleId { get; set; }

    // ======== 表单上要填的字段，配合你的 Tutor/Index.cshtml =========

    [Required(ErrorMessage = "First Name is required.")]
    public string? FirstName { get; set; }       // 👈 不要 NotMapped

    [Required(ErrorMessage = "Last Name is required.")]
    public string? LastName { get; set; }        // 👈 不要 NotMapped

    [Required(ErrorMessage = "Available date is required.")]
    [DataType(DataType.Date)]
    public DateTime? AvailableDate { get; set; } // 👈 不要 NotMapped

    [Required(ErrorMessage = "Available time is required.")]
    public string? AvailableTime { get; set; }   // 👈 不要 NotMapped

    // 用于下拉框绑定的 Subject（字符串），不映射到数据库
    [NotMapped]
    [Required(ErrorMessage = "Subject is required.")]
    public string? Subject { get; set; }

    // ======== 为了兼容 Student 区域同学写的视图而加的字段 =========

    [NotMapped]
    public string? Name { get; set; }

    [NotMapped]
    public string? Subjects { get; set; }

    [NotMapped]
    public string? Availability { get; set; }

    // =============== 导航属性（数据库关系）=================

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Schedule? Schedule { get; set; }

    public virtual Subject? SubjectNavigation { get; set; }

    public virtual AspNetUser? User { get; set; }
}
