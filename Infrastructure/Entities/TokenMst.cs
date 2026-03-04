using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class TokenMst
{
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public DateTime TokenExpiryDate { get; set; }

    public DateTime RefreshTokenExpiryDate { get; set; }

    public int CreatedBy { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int UserId { get; set; }

    public bool IsActive { get; set; }

    public bool IsDelete { get; set; }
}
