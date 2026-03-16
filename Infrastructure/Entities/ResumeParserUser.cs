using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class ResumeParserUser
{
    public int ResumeParserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
