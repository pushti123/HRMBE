using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class ApplicationQuestion
{
    public int QuestionId { get; set; }

    public int? JobApplicationId { get; set; }

    public int CandidateId { get; set; }

    public decimal? ExpectedCtc { get; set; }

    public int? NoticePeriod { get; set; }

    public bool? IsTeferral { get; set; }

    public string? Source { get; set; }

    public string? ReferralName { get; set; }
}
