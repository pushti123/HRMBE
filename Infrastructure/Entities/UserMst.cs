using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class UserMst
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? ProfilePic { get; set; }

    public string Password { get; set; } = null!;

    public string ConfirmPassword { get; set; } = null!;

    public string Contact { get; set; } = null!;

    public int? RoleId { get; set; }

    public int? DepartmentId { get; set; }

    public int? Designation { get; set; }

    public string AadharNumber { get; set; } = null!;

    public string? BankName { get; set; }

    public string? BankNumber { get; set; }

    public int CreatedBy { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsDelete { get; set; }

    public string? Address { get; set; }

    public string? Pincode { get; set; }
}
