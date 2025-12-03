using System;
using System.Collections.Generic;

namespace StudentTutoringApplication.Models;

public partial class Admin
{
    public string AdminId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
