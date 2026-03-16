using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class CandidateWorkExperience
{
    public int WorkId { get; set; }

    public int JobId { get; set; }

    public int CandidateId { get; set; }

    public string? CompanyName { get; set; }

    public string? CandidateWorkExperiencesJobTitle { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool? IsCurrentCompany { get; set; }

    public int? JobApplicationId { get; set; }

    public string? Description { get; set; }
}
