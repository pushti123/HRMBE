using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class CandidateSkill
{
    public int SkillId { get; set; }

    public int? JobApplicationId { get; set; }

    public string? SkillName { get; set; }

    public int CandidateId { get; set; }
}
