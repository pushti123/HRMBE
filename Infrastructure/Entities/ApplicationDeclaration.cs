using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class ApplicationDeclaration
{
    public int DeclarationId { get; set; }

    public int? JobApplicationId { get; set; }

    public int CandidateId { get; set; }

    public bool? AcceptedTerms { get; set; }

    public DateTime? SubmittedDate { get; set; }
}
