using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class TicketMst
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Subject { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Comment { get; set; } = null!;

    public bool Isdeleted { get; set; }

    public int CreatedBy { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? Comments { get; set; }
}
