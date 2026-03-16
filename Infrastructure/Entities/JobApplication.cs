using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class JobApplication
{
    public int JobApplicationId { get; set; }

    public int CandidateId { get; set; }

    public int ResumeId { get; set; }

    public int JobId { get; set; }

    public int ResumeParserUserId { get; set; }

    public string? JobTitle { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? IsActive { get; set; }

    public bool? FinalSubmitted { get; set; }
}
