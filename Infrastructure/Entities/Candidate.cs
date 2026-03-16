using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class Candidate
{
    public int CandidateId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? CountryCode { get; set; }

    public string? Mobile { get; set; }

    public bool? IsActive { get; set; }

    public bool? FinalSubmitted { get; set; }

    public int ResumeParserUserId { get; set; }

    public DateTime? CreatedDate { get; set; }
}
