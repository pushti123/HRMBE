using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class CandidateEducation
{
    public int EducationId { get; set; }

    public int? JobApplicationId { get; set; }

    public string? CollegeName { get; set; }

    public string? Degree { get; set; }

    public string? FieldOfStudy { get; set; }

    public int? StartYear { get; set; }

    public int? EndYear { get; set; }

    public int CandidateId { get; set; }
}
