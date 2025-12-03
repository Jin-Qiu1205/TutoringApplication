using System;
using System.Collections.Generic;
//add this
using System.ComponentModel.DataAnnotations;

namespace StudentTutoringApplication.Models;

public partial class AspNetUserLogin
{
    [Key]
    public string LoginProvider { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public string? ProviderDisplayName { get; set; }

    public string UserId { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
