using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class CandidateLanguage
{
    public int LanguageId { get; set; }

    public int? JobApplicationId { get; set; }

    public int CandidateId { get; set; }

    public string? LanguageName { get; set; }
}
