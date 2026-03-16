using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class LeaveApplication
{
    public int LeaveApplicationId { get; set; }

    public int EmployeeId { get; set; }

    public int LeaveTypeId { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public decimal TotalDays { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public DateTime? AppliedDate { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public decimal? PaidLeaveDays { get; set; }

    public decimal? LwpDays { get; set; }

    public virtual ICollection<LeaveApplicationDetail> LeaveApplicationDetails { get; set; } = new List<LeaveApplicationDetail>();

    public virtual LeaveType LeaveType { get; set; } = null!;
}
