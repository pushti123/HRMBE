using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class CandidateResume
{
    public int ResumeId { get; set; }

    public int CandidateId { get; set; }

    public int ResumeParserUserId { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public string? ParsedJson { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int JobId { get; set; }
}
